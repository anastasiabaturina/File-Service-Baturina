using FileService.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Service;

public interface IFileService
{
    Task<string> SaveFile(UploadFileRequest uploadFile);

    Task<FileStreamResult> GetFileAsync(string fileName);

    Task DeleteFile(string uniqueName, string password);

    Task AutoDeleteFile();
}
