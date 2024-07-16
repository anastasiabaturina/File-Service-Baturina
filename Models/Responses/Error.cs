using System.Net;

namespace FileService.Models.Responses;

public class Error
{
    public string? Message { get; set; }

    public HttpStatusCode ErrorCode { get; set; }
}
