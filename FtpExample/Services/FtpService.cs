using FluentFTP;
using System.Net;

namespace FtpExample.Services
{
    public class FtpService
    {       
        private readonly string _userName = string.Empty;
        private readonly string _password = string.Empty;
        private readonly AsyncFtpClient _ftp;
        private readonly ILogger<FtpService> _logger;

        public FtpService(ILogger<FtpService> logger)
        {
            _userName = "nkminipos-api";
            _password = "nkminipos@123";
            _ftp = new AsyncFtpClient
            {
                Host = "win8135.site4now.net",
                Credentials = new NetworkCredential(_userName, _password)

            };
            _logger = logger;
        }

        public async Task ConnectAsync()
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);
        }
        public async Task<bool> CheckDirectoryExistsAsync(string directory)
        {
            try
            {
                var token = new CancellationToken();
                await _ftp.Connect(token);

                return await _ftp.DirectoryExists(directory, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task UploadFileAsync(IFormFile file, string directory)
        {
            var tempFilePath = Path.GetTempFileName();
            _logger.LogInformation($"Upload File Temp File Path: {tempFilePath}");
            try
            {
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var token = new CancellationToken();
                await _ftp.Connect(token);
                _logger.LogInformation("Connected to FTP server.");

                await _ftp.CreateDirectory(directory, token);
                _logger.LogInformation($"Remote directory '{directory}' checked/created.");

                var remoteFilePath = Path.Combine(directory, file.FileName).Replace("\\", "/");
                _logger.LogInformation($"Remote file Path: {remoteFilePath}");

                var success = await _ftp.UploadFile(tempFilePath, remoteFilePath, token: token);
                _logger.LogInformation("File Uploaded Successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Upload File Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogInformation($"Upload File Inner Exception: {ex.InnerException.Message}");
                }

                throw;
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                    _logger.LogInformation("Temporary file deleted.");
                }
            }

        }
    }
}
