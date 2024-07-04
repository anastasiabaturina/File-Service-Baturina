namespace FileService.Models.UploadFileDto;

public class UploadFileDto
{
    public string Password { get; set; } 

    public IFormFile File { get; set; }
}
