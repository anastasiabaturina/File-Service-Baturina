using FileService.Models.Request;
using FileService.Models.UploadFileDto;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Service;

public interface IFileService
{
    Task<string> SaveFile(UploadFileDto uploadFile);

    Task<FileStreamResult> GetFileAsync(string fileName);

    Task DeleteFile(string uniqueName, string password);

    Task AutoDeleteFile();
}
