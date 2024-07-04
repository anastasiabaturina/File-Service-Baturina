using Microsoft.AspNetCore.Mvc;
using FileService.Models;
using Scrypt;
using System.Security.Authentication;
using Visus.Cuid;
using FileService.Models.Request;
using FileService.Models.UploadFileDto;
using FileService.Models.Dto_s;

namespace FileService.Service;

public class FileService : IFileService
{
    private readonly IRepository _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ScryptEncoder _scryptEncoder;

    public FileService(IRepository repository, IWebHostEnvironment webHostEnvironment, ScryptEncoder scryptEncoder)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _scryptEncoder = scryptEncoder;
    }

    public async Task<string> SaveFile(UploadFileDto uploadFile)
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

        await _repository.Save(file);

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

        if (!System.IO.File.Exists(filePath))
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

    //public async Task<bool> DeleteFile(DeleteFileDto deleteFileDto)
    //{
        //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, uniqueName);

        //if (!System.IO.File.Exists(filePath))
        //{
        //    throw new FileNotFoundException();
        //}

        //var passwordHash = await _repository.GetHashPassword(uniqueName);

        //if (!_scryptEncoder.Compare(password, passwordHash))
        //{
        //    throw new AuthenticationException();
        //}

        //await _repository.DeleteFile(uniqueName);

        //File.Delete(filePath);
    }

    //public async Task AutoDeleteFile()
    //{
    //    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    //    foreach (var filePath in Directory.EnumerateFiles(directoryPath))
    //    {
    //        var fileInfo = new FileInfo(filePath);

    //        if (fileInfo.CreationTime < DateTime.UtcNow.AddDays(-1))
    //        {
    //            File.Delete(filePath);
    //        }
//    //    }

//    //    await _repository.DeletionByDate();
//    }
//}
