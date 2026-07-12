using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("markets")]
    public class Market
    {
        [Key]
        [Column("market_id")]
        public int MarketId { get; set; }

        [Required]
        [Column("event_id")]
        public int EventId { get; set; }

        [Required]
        [Column("market_type")]
        [MaxLength(100)]
        public string MarketType { get; set; } = string.Empty;

        [Required]
        [Column("market_value")]
        [MaxLength(20)]
        public string MarketValue { get; set; } = string.Empty;

        [Column("is_available")]
        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
        public virtual ICollection<Selection> Selections { get; set; } = new List<Selection>();
    }
}