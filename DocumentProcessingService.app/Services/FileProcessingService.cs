using DocumentProcessingService.app.Queries;
using System.Collections.Generic;
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

        public FileProcessingService(IFileShareQuery fileShareQuery)
        {
            _fileShareQuery = fileShareQuery;
        }

        public async Task<IEnumerable<string>> ProcessFile(string fileName)
        {
            //here we process the file to retrieve the number of keywords
            var keywords = new List<string>();
            var documentContents = await _fileShareQuery.ReadFile(fileName);


            foreach (var line in documentContents.Body)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    //do stuff
                }
            }
            //TODO: implement keyword search
            return keywords;
        }
    }
}
