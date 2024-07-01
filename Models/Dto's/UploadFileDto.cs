using System.ComponentModel.DataAnnotations;

namespace File_Service
{
    public class UploadFileDto
    {
        public string Password { get; set; }
        public IFormFile File { get; set; }
    }
}
