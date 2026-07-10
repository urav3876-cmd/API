using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("events")]
    public class Event
    {
        [Key]
        [Column("event_id")]
        public Guid EventId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("sport_id")]
        public Guid SportId { get; set; }

        [Required]
        [Column("team1")]
        [MaxLength(100)]
        public string Team1 { get; set; } = string.Empty;

        [Required]
        [Column("team2")]
        [MaxLength(100)]
        public string Team2 { get; set; } = string.Empty;

        [Required]
        [Column("score")]
        [MaxLength(10)]
        public string Score { get; set; } = "0:0";

        [Required]
        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "scheduled";

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
