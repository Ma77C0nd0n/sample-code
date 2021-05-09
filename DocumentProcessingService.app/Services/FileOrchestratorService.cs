using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentProcessingService.app.Stores;

namespace DocumentProcessingService.app.Services
{

    public interface IFileOrchestratorService
    {
        Task Handle();
    }

    public class FileOrchestratorService : IFileOrchestratorService
    {
        private readonly IFileShareQuery _fileShareQuery;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly ILookupStore _lookupStore;
        private readonly IFileDeletionService _fileDeletionService;
        private readonly ILogger<FileShareQuery> _logger;
        //TODO: Extend to read base fileshare location from env var
        //TODO: Iterate through company directories rather than single path
        private const string FileshareLocalRoot = "Fileshare_local";
        private const string CompanyDirectory = "CompanyA";

        public FileOrchestratorService(IFileShareQuery fileShareQuery, 
            IFileProcessingService fileProcessingService,
            ILookupStore lookupStore,
            IFileDeletionService fileDeletionService,
            ILogger<FileShareQuery> logger)
        {
            _fileShareQuery = fileShareQuery;
            _fileProcessingService = fileProcessingService;
            _lookupStore = lookupStore;
            _fileDeletionService = fileDeletionService;
            _logger = logger;
        }

        public async Task Handle()
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\..\{FileshareLocalRoot}\{CompanyDirectory}");
            var fileNames = await _fileShareQuery.GetFileNamesForNetworkLocationAsync(relativePath);
            _logger.LogInformation($"Retrieved {fileNames.Count()} files from network path: {relativePath}");
            foreach (string fileNameWithPath in fileNames)
            {
                var fileName = Path.GetFileName(fileNameWithPath);
                if (fileName.IsValidFileName())
                {
                    string documentId = fileName.GetDocumentId();
                    string documentName = fileName.GetDocumentName();
                    var keywords = await _fileProcessingService.ProcessFile(fileNameWithPath);
                    _lookupStore.Record(documentId, documentName, keywords);
                    //future improvement - batch success files and delete after every file has been processed
                    //add retry mechanism for unsuccessful files
                    await _fileDeletionService.DeleteFileAsync(fileNameWithPath);
                }
                else
                {
                    _logger.LogWarning($"Invalid filename: {fileName}");
                }
            }
        }
    }
}
