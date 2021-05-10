using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessingService.app.Models
{
    public class DocumentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public string DocumentId { get; set; }
        [Required]
        public string Keywords { get; set; }
    }
}
