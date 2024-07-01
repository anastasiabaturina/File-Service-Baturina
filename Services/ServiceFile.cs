using Microsoft.AspNetCore.Mvc;
using Scrypt;
using System.Security.Authentication;
using Visus.Cuid;

namespace FileService.Service;

public class ServiceFile: IServiceFile
{
    private readonly IRepository _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ScryptEncoder _scryptEncoder;

    public ServiceFile(IRepository repository, IWebHostEnvironment webHostEnvironment, ScryptEncoder scryptEncoder)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _scryptEncoder = scryptEncoder;
    }

    public async Task<string> SaveFile(UploadFileDto uploadFile)
    {
        var uniqueName = $"{Cuid.NewCuid()}_{uploadFile.File.FileName}";
        var path = Path.Combine(_webHostEnvironment.WebRootPath, uniqueName);

        var fileEntity = new FileEntity
        {
            Id = new Guid(),
            UniqueName = uniqueName,
            Path = path,
            UploadDateTime = DateTime.UtcNow,
            Password = _scryptEncoder.Encode(uploadFile.Password)
        };

        await _repository.SaveFile(fileEntity);

        await using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true))
        {
            await uploadFile.File.CopyToAsync(stream); 
        }

        return uniqueName;
    }

    public async Task<FileStreamResult> GetFileAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("Имя файла не может быть пустым", nameof(fileName));
        }

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

        if (!File.Exists(filePath))
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

    public async Task DeleteFile(string uniqueName, string password)
    {
        if (string.IsNullOrEmpty(uniqueName))
        {
            throw new ArgumentException("Имя файла не может быть пустым", nameof(uniqueName));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Пароль не может быть пустым", nameof(password));
        }

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, uniqueName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException();
        }

        var passwordHash = await _repository.GetHashPassword(uniqueName);

        if (!_scryptEncoder.Compare(password, passwordHash))
        {
            throw new AuthenticationException();
        }

        await _repository.DeleteFile(uniqueName);

        File.Delete(filePath);
    }

    public async Task AutoDeleteFile()
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        foreach (var filePath in Directory.EnumerateFiles(directoryPath))
        {
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.CreationTime < DateTime.UtcNow.AddDays(-1))
            {
                File.Delete(filePath);
            }
        }

        await _repository.DeletionByDate();
    }
}
