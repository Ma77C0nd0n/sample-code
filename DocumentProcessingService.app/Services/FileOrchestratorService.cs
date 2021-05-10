using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentProcessingService.app.Stores;
using DocumentProcessingService.app.Repositories;

namespace DocumentProcessingService.app.Services
{

    public interface IFileOrchestratorService
    {
        Task DoWork();
    }

    public class FileOrchestratorService : IFileOrchestratorService
    {
        private readonly IFileShareQuery _fileShareQuery;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly ILookupStore _lookupStore;
        private readonly IFileDeletionRepository _fileDeletionService;
        private readonly ILogger<FileOrchestratorService> _logger;
        //TODO: Extend to read base fileshare location from configs
        private const string FILESHARE_LOCAL_ROOT = "..\\..\\..\\Fileshare_local\\CompaniesDirectory";

        public FileOrchestratorService(IFileShareQuery fileShareQuery, 
            IFileProcessingService fileProcessingService,
            ILookupStore lookupStore,
            IFileDeletionRepository fileDeletionService,
            ILogger<FileOrchestratorService> logger)
        {
            _fileShareQuery = fileShareQuery;
            _fileProcessingService = fileProcessingService;
            _lookupStore = lookupStore;
            _fileDeletionService = fileDeletionService;
            _logger = logger;
        }

        public async Task DoWork()
        {
            var directories = _fileShareQuery.GetDirectories(FILESHARE_LOCAL_ROOT); 
            foreach (string companyDirectory in directories)
            {
                var fileNames = await _fileShareQuery.GetFileNamesForNetworkLocationAsync(companyDirectory);
                _logger.LogInformation($"Retrieved {fileNames.Count()} files from network path: {companyDirectory}");
                
                foreach (string fileNameWithPath in fileNames)
                {
                    var fileName = Path.GetFileName(fileNameWithPath);
                    if (fileName.IsValidFileName())
                    {
                        string documentId = fileName.GetDocumentId();
                        string documentName = fileName.GetDocumentName();
                        var keywords = await _fileProcessingService.ProcessFile(fileNameWithPath);
                        if (keywords != null)
                        {
                            await _lookupStore.RecordAsync(documentId, documentName, keywords);
                            //future improvement - batch success files and delete after every file has been processed
                            await _fileDeletionService.DeleteFileAsync(fileNameWithPath);
                        }
                        else
                        {
                            _logger.LogWarning($"File processed unsuccesfully: {fileName}, result will not be stored");
                            //future improvement: add retry mechanism for unsuccessful files
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid filename: {fileName}");
                    }
                }
            }
        }
    }
}
