using FileService.Models;

namespace FileService;

public interface IRepository
{
    Task Save(Document file);

    Task<Document> Get(string uniqueName);

    Task Delete(string uniqueName);

    Task<List<Document>> GetFilesByDateTimeAsync(DateTime timeInterval);

    Task RemoveListAsync(List<Document> files);
}
