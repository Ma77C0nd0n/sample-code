using DocumentProcessingService.app.Models;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Services
{

    public interface IFileReadingService
    {
        Task<DocumentContent> ReadDataFromFile(string fileName);
    }

    public class FileReadingService : IFileReadingService
    {
        public Task<DocumentContent> ReadDataFromFile(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
