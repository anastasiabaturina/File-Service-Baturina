namespace FileService.Models.Dtos;

public class UploadFileDto
{
    public string? Password { get; set; } 

    public IFormFile? File { get; set; }
}
