using DocumentProcessingService.app.Queries;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Services
{

    public interface IFileProcessingService
    {
        Task<IEnumerable<string>> ProcessFile(string fileName);
    }

    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileShareQuery _fileShareQuery;
        private readonly ILogger<FileProcessingService> _logger;

        public FileProcessingService(IFileShareQuery fileShareQuery, ILogger<FileProcessingService> logger)
        {
            _fileShareQuery = fileShareQuery;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ProcessFile(string fileName)
        {
            var documentContents = await _fileShareQuery.ReadFile(fileName);

            if (documentContents == null)
            {
                _logger.LogWarning($"Invalid file not processed: {fileName}");
                return null;
            }
            
            foreach (var line in documentContents.Body)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parametersFound = FindParametersInLine(line, documentContents.Parameters);
                    UpdateParameterDictionary(documentContents.Parameters, parametersFound);
                }
            }
            return documentContents?.Parameters?.Keys;
        }

        private List<string> FindParametersInLine(string line, Dictionary<string, bool> paramDictionary)
        {
            return paramDictionary
                .Where(x => !x.Value && Regex.IsMatch(line, $@"\b{x.Key}\b", RegexOptions.IgnoreCase))
                .Select(y => y.Key).ToList();
        }

        private void UpdateParameterDictionary(Dictionary<string, bool> paramDictionary, List<string> parametersFound)
        {
            foreach (var found in parametersFound)
            {
                paramDictionary[found] = true;
            }
        }
    }
}
