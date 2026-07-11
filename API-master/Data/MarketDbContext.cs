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
                entity.ToTable("markets");
                entity.HasKey(e => e.MarketId);
                entity.Property(e => e.MarketId).HasColumnName("market_id").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.EventId).HasColumnName("event_id").IsRequired();
                entity.Property(e => e.MarketType).HasColumnName("market_type").IsRequired().HasMaxLength(100);
                entity.Property(e => e.MarketValue).HasColumnName("market_value").IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsAvailable).HasColumnName("is_available");

                // СВЯЗЬ С SELECTIONS
                entity.HasMany(m => m.Selections)
                    .WithOne(s => s.Market)
                    .HasForeignKey(s => s.MarketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Selection>(entity =>
            {
                entity.ToTable("selections");
                entity.HasKey(e => e.SelectionId);
                entity.Property(e => e.SelectionId).HasColumnName("selection_id").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.MarketId).HasColumnName("market_id").IsRequired();
                entity.Property(e => e.SelectionName).HasColumnName("selection_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Coefficient).HasColumnName("coefficient").IsRequired();
                entity.Property(e => e.IsWon).HasColumnName("is_won");
            });
        }
    }
}