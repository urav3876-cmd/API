using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("players")]
    public class Player
    {
        [Key]
        [Column("player_id")]
        public int PlayerId { get; set; }

        [Required]
        [Column("login")]
        [MaxLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("first_name")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("last_name")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("passport_number")]
        [MaxLength(6)]
        public string PassportNumber { get; set; } = string.Empty;

        [Required]
        [Column("passport_series")]
        [MaxLength(4)]
        public string PassportSeries { get; set; } = string.Empty;

        [Column("balance", TypeName = "decimal(16,4)")]
        public decimal Balance { get; set; } = 0.0000m;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        //навигационные свойства
        public ICollection<Bet> Bets { get; set; } = new List<Bet>();
    }
}
