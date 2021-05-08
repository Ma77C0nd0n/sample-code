using System.Collections.Generic;

namespace DocumentService.Models
{
    public class DocumentContent
    {
        public string Client { get; set; }
        public string DocumentId { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}
