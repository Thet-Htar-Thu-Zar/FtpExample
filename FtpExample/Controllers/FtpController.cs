using FtpExample.Models;
using FtpExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace FtpExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtpController : ControllerBase
    {

        private readonly FtpService _ftpService;

        public FtpController(FtpService ftpService)
        {
            _ftpService = ftpService;
        }

        [HttpGet]
        public async Task<IActionResult> CheckFtpDirector(string directoryName)
        {
            try
            {
                bool isExist = await _ftpService.CheckDirectoryExistsAsync(directoryName);
                return Ok(isExist);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] FtpRequestModel requestModel)
        {
            try
            {
                await _ftpService.UploadFileAsync(requestModel.File, requestModel.DirectoryName);
                return Ok(requestModel);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
