using System.ComponentModel.DataAnnotations;

namespace FileService;

public class UploadFileDto
{
    public string Password { get; set; }
    public IFormFile File { get; set; }
}
