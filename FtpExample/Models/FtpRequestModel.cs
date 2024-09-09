using System.ComponentModel.DataAnnotations;

namespace FtpExample.Models
{
    public class FtpRequestModel
    {     
        public required IFormFile File { get; set; }
        public required string DirectoryName { get; set; }
    }
}
