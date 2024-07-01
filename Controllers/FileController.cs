using File_Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace File_Service.Controllers
{
    [ApiController]
    [Route("File")]
    public class FileController : ControllerBase
    {
        private readonly IServiceFile _serviceFile;

        public FileController(IServiceFile serviceFile)
        {
            _serviceFile = serviceFile;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileDto uploadFile)
        {
            var response = new Response<string>
            {              
                Data = await _serviceFile.SaveFile(uploadFile),
                StatusCode = StatusCodes.Status201Created,
                Success = true,
            };

            return Ok(response);
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
        { 
            try
            {
                return await _serviceFile.GetFileAsync(fileName);
            }

            catch (ArgumentException ex)
            {
                var response = new Response<string>
                {
                    Data = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false
                };

                return BadRequest();
            }

            catch (FileNotFoundException)
            {
                var response = new Response<string>
                {
                    Data = "Файл не найден",
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false
                };

                return NotFound(response);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string uniqueName, string password)
        {
            try
            {
                await _serviceFile.DeleteFile(uniqueName, password);

                var response = new Response<string>
                {
                    Data = "Файл успешно удален",
                    StatusCode = StatusCodes.Status200OK,
                    Success = true
                };

                return Ok(response);
            }

            catch (ArgumentException ex)
            {
                var response = new Response<string>
                {
                    Data = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false
                };

                return BadRequest();
            }

            catch (FileNotFoundException)
            {
                var response = new Response<string>
                {
                    Data = "Файл не найден",
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false
                };

                return NotFound(response);
            }

            catch (AuthenticationException)
            {
                var response = new Response<string>
                {
                    Data = "Неверный пароль",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Success = false
                };

                return Unauthorized(response);
            }
        }
    }
}
