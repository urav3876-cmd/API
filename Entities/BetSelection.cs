using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("betselections")]
    public class BetSelection
    {
        [Key]
        [Column("bet_selection_id")]
        public int BetSelectionId { get; set; }

        [Required]
        [Column("bet_id")]
        public int BetId { get; set; }

        [Required]
        [Column("selection_id")]
        public long SelectionId { get; set; }

        [Required]
        [Column("market_id")]
        public int MarketId { get; set; }

        [Required]
        [Column("selection_name")]
        [MaxLength(50)]
        public string SelectionName { get; set; } = string.Empty;

        [Required]
        [Column("coefficient", TypeName = "decimal(10,6)")]
        public decimal Coefficient { get; set; }

        [Column("is_won")]
        public bool? IsWon { get; set; }

        //навигационные свойства
        [ForeignKey("BetId")]
        public Bet Bet { get; set; } = null!;
    }
}
