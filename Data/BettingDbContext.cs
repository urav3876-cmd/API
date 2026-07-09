using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class BettingDbContext : DbContext
    {
        public BettingDbContext(DbContextOptions<BettingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<BetSelection> BetSelections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasIndex(p => p.Login).IsUnique();
            });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Bets_Status",
                    "status IN ('pending', 'won', 'lost', 'refunded', 'cancelled')"));

                entity.HasOne(b => b.Player)
                      .WithMany(p => p.Bets)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BetSelection>(entity =>
            {
                entity.HasOne(s => s.Bet)
                      .WithMany(b => b.Selections)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}