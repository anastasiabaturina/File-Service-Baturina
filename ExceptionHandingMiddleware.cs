using FileService.Models.Responses;
using System.Net;

namespace FileService;

public class ExceptionHandingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ArgumentException ex)
        {
            var code = HttpStatusCode.BadRequest;
            var errorMessage = ex.Message;
            await HandleExceptionAsync(context, code, errorMessage);
        }
        catch(FileNotFoundException)
        {
            var code = HttpStatusCode.NotFound;
            var errorMessage = "File not found.";
            await HandleExceptionAsync(context, code, errorMessage);
        }
        catch (Exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errorMessage = "Internal server error";
            await HandleExceptionAsync(context, code, errorMessage);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string errorMessage)
    {     
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
        return context.Response.WriteAsJsonAsync(response);
    }
}

