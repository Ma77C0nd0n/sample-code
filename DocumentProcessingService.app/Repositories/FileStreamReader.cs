using System.IO;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Repositories
{
    public interface IFileStreamReader
    {
        Task<string> ReadToEndAsync(string path);
    }

    public class FileStreamReader : IFileStreamReader
    {
        public async Task<string> ReadToEndAsync(string path)
        {
            using var reader = new StreamReader(path);
            return await reader.ReadToEndAsync();
        }
    }
}
