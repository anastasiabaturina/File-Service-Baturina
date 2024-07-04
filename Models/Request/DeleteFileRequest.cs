namespace FileService.Models.Request;

public class DeleteFileRequest
{
    public string UniqueName { get; set; }

    public string Password { get; set; }
}
