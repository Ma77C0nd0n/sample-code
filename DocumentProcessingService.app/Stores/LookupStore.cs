using DocumentProcessingService.app.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentProcessingService.app.Stores
{
    public interface ILookupStore
    {
        /// <summary> 
        /// Records set of keywords identified in the given document for a given client 
        /// </summary> 
        /// <param name="client">Client identifier</param> 
        /// <param name="documentId">Document identifier</param> 
        /// <param name="keywords">Enumeration of unique keywords found in the document, in any
        /// order. Only match exact words, not prefix match. </param> 
        Task RecordAsync(string client, string documentId, IEnumerable<string> keywords);
    }

    public class LookupStore : ILookupStore
    {
        private readonly DocumentContext _context;
        private readonly IMemoryCache _memoryCache;

        private readonly ILogger<LookupStore> _logger;

        public LookupStore(DocumentContext context, IMemoryCache memoryCache, ILogger<LookupStore> logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task RecordAsync(string client, string documentId, IEnumerable<string> keywords)
        {
            var documentItem = new DocumentItem
            {
                Client = client,
                DocumentId = documentId,
                Keywords = string.Join(',', keywords)
            };

            _context.DocumentItems.Add(documentItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully persisted result for client: {client}, document: {documentId}");
        }
    }
}
