using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Sport> Sports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.ToTable("sports");
                entity.HasKey(e => e.SportId);
                entity.Property(e => e.SportId).HasColumnName("sport_id").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.SportName).HasColumnName("sport_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasIndex(e => e.SportName).IsUnique();
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).HasColumnName("event_id").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.SportName).HasColumnName("sport_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Team1).HasColumnName("team1").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Team2).HasColumnName("team2").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Score).HasColumnName("score").HasMaxLength(10).HasDefaultValue("0:0");
                entity.Property(e => e.Status).HasColumnName("status").IsRequired().HasMaxLength(20).HasDefaultValue("scheduled");
                entity.Property(e => e.StartTime).HasColumnName("start_time").IsRequired();
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.Sport)
                    .WithMany(s => s.Events)
                    .HasForeignKey(e => e.SportName)
                    .HasPrincipalKey(s => s.SportName)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}