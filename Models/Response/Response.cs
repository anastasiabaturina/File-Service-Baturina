namespace FileService.Models.Response;

public class Response<T>
{
    public T? Data { get; set; }

    public int StatusCode { get; set; }

    public bool Success { get; set; }
}

