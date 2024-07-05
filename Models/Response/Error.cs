using System.Net;

namespace FileService.Models.Response;

public class Error
{
    public string? Message { get; set; }

    public HttpStatusCode ErrorCode { get; set; }
}
