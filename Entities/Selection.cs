using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("selections")]
    public class Selection
    {
        [Key]
        [Column("selection_id")]
        public int SelectionId { get; set; }

        [Required]
        [Column("market_id")]
        public int MarketId { get; set; }

        [Required]
        [Column("selection_name")]
        [MaxLength(100)]
        public string SelectionName { get; set; } = string.Empty;

        [Required]
        [Column("coefficient", TypeName = "decimal(10,6)")]
        public decimal Coefficient { get; set; }

        [Column("is_won")]
        public bool IsWon { get; set; }

        [ForeignKey(nameof(MarketId))]
        public virtual Market Market { get; set; } = null!;
    }
}