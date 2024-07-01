namespace FileService;

public class UploadFileRequest
{
    public string Password { get; set; }

    public IFormFile File { get; set; }
}
