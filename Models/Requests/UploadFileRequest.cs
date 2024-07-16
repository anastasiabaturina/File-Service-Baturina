namespace FileService.Models.Requests;

public class UploadFileRequest
{
    public string Password { get; set; } = null!;

    public IFormFile File { get; set; } = null!;
}
