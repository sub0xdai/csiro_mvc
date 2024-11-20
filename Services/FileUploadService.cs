using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Serilog;
using ILogger = Serilog.ILogger;
using Microsoft.Extensions.Configuration;

namespace csiro_mvc.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string subDirectory);
        Task<bool> DeleteFileAsync(string filePath);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly string _baseUploadPath;
        private readonly ILogger _logger;

        public FileUploadService(IConfiguration configuration)
        {
            _baseUploadPath = configuration["FileUpload:BasePath"] ?? "wwwroot/uploads";
            _logger = Log.ForContext<FileUploadService>();
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subDirectory)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null", nameof(file));
                }

                // Create the upload directory if it doesn't exist
                var uploadPath = Path.Combine(_baseUploadPath, subDirectory);
                Directory.CreateDirectory(uploadPath);

                // Generate a unique filename
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                _logger.Information("Uploading file: {FileName} to {FilePath}", file.FileName, filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error uploading file: {FileName}", file.FileName);
                throw;
            }
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.Information("File deleted successfully: {FilePath}", filePath);
                    return Task.FromResult(true);
                }
                
                _logger.Warning("File not found for deletion: {FilePath}", filePath);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting file: {FilePath}", filePath);
                throw;
            }
        }
    }
}
