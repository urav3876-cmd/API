using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("outcomes")]
    public class Outcome
    {
        [Key]
        [Column("outcome_id")]
        public int OutcomeId { get; set; }

        [Required]
        [Column("event_id")]
        public int EventId { get; set; }

        [Required]
        [Column("outcome_name")]
        [MaxLength(100)]
        public string OutcomeName { get; set; } = string.Empty;

        [Required]
        [Column("outcome_score")]
        [MaxLength(10)]
        public string OutcomeScore { get; set; } = string.Empty;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
    }
}