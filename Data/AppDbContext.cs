using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Selection> Selections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SPORT 
            modelBuilder.Entity<Sport>(entity =>
            {
                entity.HasIndex(e => e.SportName).IsUnique();
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // EVENT
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(e => e.Sport)
                    .WithMany(s => s.Events)
                    .HasForeignKey(e => e.SportId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Event_Status",
                    "status IN ('upcoming', 'live', 'finished', 'cancelled')"));

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // OUTCOME
            modelBuilder.Entity<Outcome>(entity =>
            {
                entity.HasOne(o => o.Event)
                    .WithMany(e => e.Outcomes)
                    .HasForeignKey(o => o.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // MARKET
            modelBuilder.Entity<Market>(entity =>
            {
                entity.HasOne(m => m.Event)
                    .WithMany(e => e.Markets)
                    .HasForeignKey(m => m.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.Selections)
                    .WithOne(s => s.Market)
                    .HasForeignKey(s => s.MarketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SELECTION
            modelBuilder.Entity<Selection>(entity =>
            {
                entity.HasOne(s => s.Market)
                    .WithMany(m => m.Selections)
                    .HasForeignKey(s => s.MarketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}