using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("events")]
    public class Event
    {
        [Key]
        [Column("event_id")]
        public int EventId { get; set; }

        [Required]
        [Column("sport_id")]
        public int SportId { get; set; }

        [Required]
        [Column("team1")]
        [MaxLength(100)]
        public string Team1 { get; set; } = string.Empty;

        [Required]
        [Column("team2")]
        [MaxLength(100)]
        public string Team2 { get; set; } = string.Empty;

        [Column("score")]
        [MaxLength(10)]
        public string Score { get; set; } = "0:0";

        [Required]
        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "upcoming";

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(SportId))]
        public virtual Sport Sport { get; set; } = null!;

        public virtual ICollection<Outcome> Outcomes { get; set; } = new List<Outcome>();
        public virtual ICollection<Market> Markets { get; set; } = new List<Market>();
    }
}