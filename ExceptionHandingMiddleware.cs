using FileService.Models.Response;
using System.Net;

namespace FileService;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; 
        var errorMessage = "Internal server error";

        if (exception is ArgumentException)
        {
            code = HttpStatusCode.BadRequest;
            errorMessage = exception.Message;
        }
        else if (exception is FileNotFoundException)
        {
            code = HttpStatusCode.NotFound;
            errorMessage = "File not found.";
        }

        var response = new Response<object>
        {
            Data = null,
            Error = new Error
            {
                Message = errorMessage,
                ErrorCode = code
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}

