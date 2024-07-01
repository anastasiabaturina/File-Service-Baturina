using Microsoft.AspNetCore.Mvc;

namespace File_Service.Service
{
    public interface IServiceFile
    {
        Task<string> SaveFile(UploadFileDto uploadFile);

        Task<FileStreamResult> GetFileAsync(string fileName);

        Task DeleteFile(string uniqueName, string password);

        Task AutoDeleteFile();
    }
}
