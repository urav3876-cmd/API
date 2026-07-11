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
        public Guid SelectionId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("market_id")]
        public Guid MarketId { get; set; }

        [Required]
        [Column("selection_name")]
        [MaxLength(100)]
        public string SelectionName { get; set; } = string.Empty;

        [Required]
        [Column("coefficient")]
        public decimal Coefficient { get; set; }

        [Column("is_won")]
        public bool IsWon { get; set; }

        [ForeignKey(nameof(MarketId))]
        public virtual Market Market { get; set; } = null!;
    }
}