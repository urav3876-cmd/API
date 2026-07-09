using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("bets")]
    public class Bet
    {
        [Key]
        [Column("bet_id")]
        public int BetId { get; set; }

        [Required]
        [Column("player_id")]
        public int PlayerId { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(16,4)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("calculated_payout", TypeName = "decimal(16,4)")]
        public decimal CalculatedPayout { get; set; }

        [Required]
        [Column("actual_payout", TypeName = "decimal(16,4)")]
        public decimal ActualPayout { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("settled_at")]
        public DateTime? SettledAt { get; set; }

        //навигационные свойства
        [ForeignKey("PlayerId")]
        public Player Player { get; set; } = null!;

        public ICollection<BetSelection> Selections { get; set; } = new List<BetSelection>();
    }
}