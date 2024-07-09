using FileService.Models;
using FileService.Models.Dto_s;
using FileService.Models.Response;
using FileService.Models.UploadFileDto;

namespace FileService.Service;

public interface IFileService
{
    Task<UploadFileResponse> SaveAsync(UploadFileDto uploadFile, CancellationToken cancellationToken);

    Task<FileDto> GetAsync(string fileName, CancellationToken cancellationToken);

    Task DeleteAsync(DeleteFileDto deleteFileDto, CancellationToken cancellationToken);

    Task AutoDeleteFilesAsync(CancellationToken cancellationToken);
}