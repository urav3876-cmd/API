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
        public Guid MarketId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("event_id")]
        public Guid EventId { get; set; }

        [Required]
        [Column("market_type")]
        [MaxLength(100)]
        public string MarketType { get; set; } = string.Empty;

        [Required]
        [Column("market_value")]
        [MaxLength(100)]
        public string MarketValue { get; set; } = string.Empty;

        [Column("is_available")]
        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
        public virtual ICollection<Selection> Selections { get; set; } = new List<Selection>();
    }
}