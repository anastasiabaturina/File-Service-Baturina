namespace FileService;

public interface IRepository
{
    Task SaveFile(File file);
    Task<string> GetHashPassword(string uniqueName);
    Task DeleteFile(string uniqueName);
    Task DeletionByDate();
}
