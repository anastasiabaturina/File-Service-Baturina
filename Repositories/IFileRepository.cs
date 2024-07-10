using FileService.Models;

namespace FileService;

public interface IFileRepository
{
    Task SaveAsync(Document document, CancellationToken cancellationToken);

    Task<Document> GetAsync(string uniqueName, CancellationToken cancellationToken);

    Task DeleteAsync(Document document, CancellationToken cancellationToken);

    Task DeleteFilesByDateTimeAsync(DateTime timeInterval, CancellationToken cancellationToken);
}
