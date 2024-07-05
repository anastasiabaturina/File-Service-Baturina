namespace FileService.Models.Dto_s;

public class FileDto
{
    public string? FileName { get; set; }

    public byte[]? Content { get; set; }

    public string? ContentType { get; set; }
}
