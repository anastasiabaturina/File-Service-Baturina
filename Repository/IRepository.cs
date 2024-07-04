using FileService.Models;

namespace FileService;

public interface IRepository
{
    Task SaveFile(Document file);

    Task<string> GetHashPassword(string uniqueName);

    Task DeleteFile(string uniqueName);

    Task DeletionByDate();
}
