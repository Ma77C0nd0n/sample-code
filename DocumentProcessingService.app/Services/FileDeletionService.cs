using System.Threading.Tasks;

namespace DocumentProcessingService.app.Services
{

    public interface IFileDeletionService
    {
        Task DeleteFileAsync(string fileName);
    }

    public class FileDeletionService : IFileDeletionService
    {
        public Task DeleteFileAsync(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
