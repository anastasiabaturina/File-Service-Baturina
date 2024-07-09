using FileService.Models.Dtos;
using FileService.Models.Response;

namespace FileService.Services;

public interface IFileService
{
    Task<UploadFileResponse> SaveAsync(UploadFileDto uploadFile, CancellationToken cancellationToken);

    Task<FileDto> GetAsync(string fileName, CancellationToken cancellationToken);

    Task DeleteAsync(DeleteFileDto deleteFileDto, CancellationToken cancellationToken);

    Task AutoDeleteFilesAsync(CancellationToken cancellationToken);
}