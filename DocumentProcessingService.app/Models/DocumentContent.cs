using System.Collections.Generic;

namespace DocumentProcessingService.app.Models
{
    public class DocumentContent
    {
        public string ProcessingType { get; set; }
        public Dictionary<string, bool> Parameters { get; set; }
        public IEnumerable<string> Body { get; set; }
    }
}
