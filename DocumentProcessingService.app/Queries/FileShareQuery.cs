using DocumentProcessingService.app.Models;
using DocumentProcessingService.app.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Queries
{
    public interface IFileShareQuery
    {
        Task<IEnumerable<string>> GetFileNamesForNetworkLocationAsync(string path);
        Task<DocumentContent> ReadFile(string fileName);
    }

    public class FileShareQuery : IFileShareQuery
    {
        private readonly IFileStreamReader _fileStreamReader;
        private readonly ILogger<FileShareQuery> _logger;

        public FileShareQuery(
            IFileStreamReader fileStreamReader,
            ILogger<FileShareQuery> logger)
        {
            _fileStreamReader = fileStreamReader;
            _logger = logger;
        }

        public Task<IEnumerable<string>> GetFileNamesForNetworkLocationAsync(string path)
        {
            _logger.LogDebug($"Retrieving files from network location: {path}");
            return Task.Run(() => { 

                try
                {
                    var files = Directory.GetFiles(path);
                    return files.AsEnumerable();
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, "ERR: The caller does not have the required permission to this directory. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (DirectoryNotFoundException ex)
                {
                    _logger.LogError(ex, "ERR: The directory path is invalid. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "ERR: Cannot open the directory path '{Path}'. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "ERR: Directory path is badly formatted. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
            });
        }

        public async Task<DocumentContent> ReadFile(string fileName)
        {
            var fileContent = await ReadFileContentAsync(fileName);
            var allLines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (allLines.Length >= 2)
            {
                var processingParams = allLines.First();
                var body = allLines.Skip(1);
                return MapDocumentContentToModel(processingParams, body, fileName);
            }
            return InvalidDocumentContent(fileName);
        }

        private DocumentContent MapDocumentContentToModel(string processingParams, IEnumerable<string> body, string fileName)
        {
            var splitParams = processingParams.Split('|');
            if(splitParams?.Length == 2)
            {
                var paramList = splitParams.Last()?.Split(',');
                var paramDictionary = paramList.Distinct(StringComparer.OrdinalIgnoreCase).ToDictionary(x => x, y => false);
                return new DocumentContent
                {
                    ProcessingType = splitParams.First(),
                    Parameters = paramDictionary,
                    Body = body
                };
            }
            return InvalidDocumentContent(fileName);
        }

        private DocumentContent InvalidDocumentContent(string fileName)
        {
            _logger.LogWarning($"File format invalid: {fileName}");
            return null;
        }

        private async Task<string> ReadFileContentAsync(string fileNameWithPath)
        {
            _logger.LogInformation($"Starting Reading of File from File Share --- File name : {fileNameWithPath}");
            
            var fileContent = await _fileStreamReader.ReadToEndAsync(fileNameWithPath);

            if (string.IsNullOrEmpty(fileContent))
            {
                throw new ArgumentException($"{nameof(fileContent)} contains no data. Expected result is for file to have data");
            }
            _logger.LogInformation($"Successfully Read File {fileNameWithPath}");

            return fileContent;
        }
    }
}
