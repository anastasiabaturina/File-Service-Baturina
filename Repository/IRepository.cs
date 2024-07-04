using FileService.Models;

namespace FileService;

public interface IRepository
{
    Task Save(Document file);

    Task<string> GetHashPassword(string uniqueName);

    Task Delete(string uniqueName);

    Task DeletionByDate();
}
