using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Selection> Selections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Market>(entity =>
            {
                entity.HasMany(m => m.Selections)
                    .WithOne(s => s.Market)
                    .HasForeignKey(s => s.MarketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}