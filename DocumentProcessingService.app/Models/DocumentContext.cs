using Microsoft.EntityFrameworkCore;

namespace DocumentProcessingService.app.Models
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options) 
            : base(options) {}

        public DocumentContext() { }

        public virtual DbSet<DocumentItem> DocumentItems { get; set; }
    }
}
