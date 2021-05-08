using DocumentService.Models;
using System.Threading.Tasks;

namespace DocumentService.Services
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
