using System.Collections.Generic;

namespace DocumentProcessingService.app.Models
{
    public class DocumentContent
    {
        public string Client { get; set; }
        public string DocumentId { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}
