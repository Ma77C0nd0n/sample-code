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
            _logger.LogDebug("---- Retrieving files from network location: {unc}", path);
            return Task.Run(() => { 

                try
                {
                    var files = Directory.GetFiles(path);
                    return files.AsEnumerable();
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, "---- ERR: The caller does not have the required permission to this directory. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (DirectoryNotFoundException ex)
                {
                    _logger.LogError(ex, "---- ERR: The directory path is invalid. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "---- ERR: Cannot open the directory path '{Path}'. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "---- ERR: Directory path is badly formatted. Path: {Path}. Message: {Message}",
                        path,
                        ex.Message);
                    throw;
                }
            });
        }

        public async Task<DocumentContent> ReadFile(string fileName)
        {
            var fileContent = await ReadFileContentAsync(fileName);

            DocumentContent documentContent = new DocumentContent();

            var allLines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            //TODO
            //do first line stuff first
            foreach (var line in allLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    //do stuff
                }
            }

            return documentContent;
        }

        private async Task<string> ReadFileContentAsync(string fileNameWithPath)
        {
            _logger.LogInformation("--- Starting Reading of File from File Share --- File name : {fileName}", fileNameWithPath);

            var fileContent = await _fileStreamReader.ReadToEndAsync(fileNameWithPath);

            if (string.IsNullOrEmpty(fileContent))
            {
                throw new ArgumentException($"{nameof(fileContent)} contains no data. Expected result is for file to have data");
            }
            _logger.LogInformation("--- Successfully Read File ---");

            return fileContent;
        }
    }
}
