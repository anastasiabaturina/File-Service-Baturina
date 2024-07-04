using FileService.Models.Dto_s;
using FileService.Models.Request;
using FileService.Models.UploadFileDto;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Service;

public interface IFileService
{
    Task<string> SaveFile(UploadFileDto uploadFile);

    Task<FileStreamResult> GetFileAsync(string fileName);

    Task<bool> DeleteFile(DeleteFileDto deleteFileDto);

    Task AutoDeleteFile();
}
