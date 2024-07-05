using FileService.Models;

namespace FileService;

public interface IRepository
{
    Task SaveAsync(Document file);

    Task<Document> GetAsync(string uniqueName);

    Task DeleteAsync(string uniqueName);

    Task DeleteFilesByDateTimeAsync(DateTime timeInterval);
}
