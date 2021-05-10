using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Repositories
{

    public interface IFileDeletionRepository
    {
        Task DeleteFileAsync(string fileName);
    }

    public class FileDeletionRepository : IFileDeletionRepository
    {
        private readonly ILogger<FileDeletionRepository> _logger;
        
        public FileDeletionRepository(ILogger<FileDeletionRepository> logger)
        {
            _logger = logger;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                await Task.Run(() =>
                {
                    _logger.LogDebug($"File to delete: {fileName}");
                    
                    DeleteFile(fileName);

                    _logger.LogDebug($"Successfully deleted file: {fileName}");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERR: Could not delete file {fileName}");
            }
        }

        protected void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
