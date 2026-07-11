using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    [Table("sports")]
    [Index(nameof(SportName), IsUnique = true)] 
    public class Sport
    {
        [Key]
        [Column("sport_id")]
        public Guid SportId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("sport_name")]
        [MaxLength(100)]
        public string SportName { get; set; } = string.Empty;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}