namespace File_Service
{
    public interface IRepository
    {
        Task SaveFile(FileEntity file);
        Task<string> GetHashPassword(string uniqueName);
        Task DeleteFile(string uniqueName);
        Task DeletionByDate();
    }
}
