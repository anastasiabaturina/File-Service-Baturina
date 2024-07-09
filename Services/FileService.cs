using FileService.Models;
using Scrypt;  
using Visus.Cuid;
using FileService.Models.Dtos;
using FileService.Models.Responses;
using AutoMapper;

namespace FileService.Services;

public class FileService : IFileService
{
    private readonly IRepository _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ScryptEncoder _scryptEncoder;
    private readonly int _timeInterval;
    private readonly IMapper _mapper;

    public FileService(IRepository repository, IWebHostEnvironment webHostEnvironment, ScryptEncoder scryptEncoder, IConfiguration configuration, IMapper mapper)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _scryptEncoder = scryptEncoder;
        _timeInterval = configuration.GetValue<int>("Time:Day");
        _mapper = mapper;
    }

    public async Task<UploadFileResponse> SaveAsync(UploadFileDto uploadFile, CancellationToken cancellationToken)
    {
        var uniqueName = $"{Cuid.NewCuid()}_{uploadFile.File.FileName.Replace(" ", "_")}";
        var path = Path.Combine(_webHostEnvironment.WebRootPath, uniqueName);

        var file = new Document
        {
            UniqueName = uniqueName,
            Path = path,
            UploadDateTime = DateTime.UtcNow,
            Password = _scryptEncoder.Encode(uploadFile.Password)
        };      

        await using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true))
        {
            await uploadFile.File.CopyToAsync(stream);
        }

        await _repository.SaveAsync(file, cancellationToken);

        var uploadFileResponse = _mapper.Map<UploadFileResponse>(file);

        return uploadFileResponse;
    }

    public async Task<FileDto> GetAsync(string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("The file name cannot be empty");
        }

        var document = await _repository.GetAsync(fileName, cancellationToken);

        if (document == null)
        {
            throw new FileNotFoundException();
        }

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

        byte[] fileBytes;
        await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
        {
            fileBytes = new byte[stream.Length];
            await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
        }

        return new FileDto
        {
            FileName = fileName,
            Content = fileBytes,
            ContentType = System.Net.Mime.MediaTypeNames.Application.Octet
        };
    }

    public async Task DeleteAsync(DeleteFileDto deleteFileDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(deleteFileDto.UniqueName))
        {
            throw new ArgumentException("The file name cannot be empty");
        }

        if (string.IsNullOrEmpty(deleteFileDto.Password))
        {
            throw new ArgumentException("The password cannot be empty");
        }

        var document = await _repository.GetAsync(deleteFileDto.UniqueName, cancellationToken);

        if (!_scryptEncoder.Compare(deleteFileDto.Password, document.Password))
        {
            throw new ArgumentException("Invalid password");
        }

        await _repository.DeleteAsync(deleteFileDto.UniqueName, cancellationToken);

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, deleteFileDto.UniqueName);

        File.Delete(filePath);
    }


    public async Task AutoDeleteFilesAsync(CancellationToken cancellationToken)
    {
        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath);
        var directory = new DirectoryInfo(directoryPath);
        var dateTimeInterval = DateTime.UtcNow.AddDays(-_timeInterval);

        await _repository.DeleteFilesByDateTimeAsync(dateTimeInterval, cancellationToken);

        foreach (var file in directory.GetFiles())
        {
            if (file.CreationTime < dateTimeInterval)
            {
                file.Delete();
            }
        }        
    }
}