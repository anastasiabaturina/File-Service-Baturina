using FileService.Models.Dto_s;
using FileService.Models.Request;
using FileService.Models.UploadFileDto;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Service;

public interface IFileService
{
    Task<string> SaveAsync(UploadFileDto uploadFile, CancellationToken cancellationToken);

    Task<FileDto> GetAsync(string fileName);

    Task DeleteAsync(DeleteFileDto deleteFileDto, CancellationToken cancellationToken);

    Task AutoDeleteFilesAsync(CancellationToken cancellationToken);
}