using Microsoft.AspNetCore.Mvc;
using FileService.Models;
using Scrypt;  
using Visus.Cuid;
using FileService.Models.UploadFileDto;
using FileService.Models.Dto_s;

namespace FileService.Service;

public class FileService : IFileService
{
    private readonly IRepository _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ScryptEncoder _scryptEncoder;
    private readonly int _timeInterval;

    public FileService(IRepository repository, IWebHostEnvironment webHostEnvironment, ScryptEncoder scryptEncoder, IConfiguration configuration)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _scryptEncoder = scryptEncoder;
        _timeInterval = configuration.GetValue<int>("Time:Day");
    }

    public async Task<string> SaveAsync(UploadFileDto uploadFile, CancellationToken cancellationToken)
    {
        var uniqueName = $"{Cuid.NewCuid()}_{uploadFile.File.FileName}";
        var path = Path.Combine(_webHostEnvironment.WebRootPath, uniqueName);

        var file = new Document
        {
            UniqueName = uniqueName,
            Path = path,
            UploadDateTime = DateTime.UtcNow,
            Password = _scryptEncoder.Encode(uploadFile.Password)
        };

        await _repository.SaveAsync(file, cancellationToken);

        await using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true))
        {
            await uploadFile.File.CopyToAsync(stream);
        }

        return uniqueName;
    }

    public async Task<FileStreamResult> GetAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("Имя файла не может быть пустым", nameof(fileName));
        }

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

        if (File.Exists(filePath))
        {
            throw new FileNotFoundException();
        }

        await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
        {
            return new FileStreamResult(stream, System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = fileName
            };
        }
    }

    public async Task DeleteAsync(DeleteFileDto deleteFileDto, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, deleteFileDto.UniqueName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("the file was not found");
        }

        var document = await _repository.GetAsync(deleteFileDto.UniqueName, cancellationToken);

        if (!_scryptEncoder.Compare(deleteFileDto.Password, document.Password))
        {
            throw new ArgumentOutOfRangeException("invalid password");
        }

        await _repository.DeleteAsync(deleteFileDto.UniqueName, cancellationToken);

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