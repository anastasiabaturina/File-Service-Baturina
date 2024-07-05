using FileService.Models;

namespace FileService;

public interface IRepository
{
    Task SaveAsync(Document file, CancellationToken cancellationToken);

    Task<Document> GetAsync(string uniqueName, CancellationToken cancellationToken);

    Task DeleteAsync(string uniqueName, CancellationToken cancellationToken);

    Task DeleteFilesByDateTimeAsync(DateTime timeInterval, CancellationToken cancellationToken);
}
