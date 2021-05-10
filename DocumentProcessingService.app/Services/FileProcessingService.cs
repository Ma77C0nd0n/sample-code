using DocumentProcessingService.app.Queries;
using Microsoft.Extensions.Logging;
using System;
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
        private const string LOOKUP_PROCESSING_TYPE = "lookup";

        public FileProcessingService(IFileShareQuery fileShareQuery, ILogger<FileProcessingService> logger)
        {
            _fileShareQuery = fileShareQuery;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ProcessFile(string fileName)
        {
            var documentContents = await _fileShareQuery.ReadFile(fileName);

            if (documentContents != null && documentContents.ProcessingType.Equals(LOOKUP_PROCESSING_TYPE, StringComparison.Ordinal))
            {
                return ProcessLookupType(documentContents.Parameters, documentContents.Body);
            }
            else
            {
                _logger.LogWarning($"Invalid file not processed: {fileName}");
                return null;
            }
        }

        private IEnumerable<string> ProcessLookupType(Dictionary<string, bool> parameters, IEnumerable<string> body)
        {
            foreach (var line in body)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parametersFound = FindParametersInLine(line, parameters);
                    UpdateParameterDictionary(parameters, parametersFound);
                }
            }
            return parameters.Where(x => x.Value).Select(x => x.Key);
        }

        private List<string> FindParametersInLine(string line, Dictionary<string, bool> paramDictionary)
        {
            var a = paramDictionary
                .Where(x => !x.Value && Regex.IsMatch(line, $@"\b{x.Key}\b", RegexOptions.IgnoreCase))
                .Select(y => y.Key).ToList();
            return a;
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
