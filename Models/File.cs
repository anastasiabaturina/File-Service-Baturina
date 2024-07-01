namespace FileService.Models;

public class File
{
    public Guid Id { get; set; }

    public string UniqueName { get; set; }

    public string? Path { get; set; }

    public DateTime UploadDateTime { get; set; }

    public string? Password { get; set; }

}

