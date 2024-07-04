using FileService.Models.Request;
using FileService.Models.Response;
using FileService.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace FileService.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest uploadFileRequest)
    {
        var response = new Response<string>
        {              
            Data = await _fileService.SaveFile(uploadFileRequest),
        };

        return CreatedAtAction(nameof(UploadFile), response);
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
    { 
        try
        {
            return await _fileService.GetFileAsync(fileName);
        }

        catch (ArgumentException ex)
        {
            var response = new Response<string>
            {
                Data = ex.Message,
            };

            return BadRequest();
        }

        catch (FileNotFoundException)
        {
            var response = new Response<string>
            {
                Data = "Файл не найден",
            };

            return NotFound(response);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile(string uniqueName, string password)
    {
        try
        {
            await _fileService.DeleteFile(uniqueName, password);

            var response = new Response<string>
            {
                Data = "Файл успешно удален",
            };

            return Ok(response);
        }

        catch (ArgumentException ex)
        {
            var response = new Response<string>
            {
                Data = ex.Message,
            };

            return BadRequest();
        }

        catch (FileNotFoundException)
        {
            var response = new Response<string>
            {
                Data = "Файл не найден",
            };

            return NotFound(response);
        }

        catch (AuthenticationException)
        {
            var response = new Response<string>
            {
                Data = "Неверный пароль",
            };

            return Unauthorized(response);
        }
    }
}
