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
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint(
                   "CK_Event_Status",
                   "status IN ('scheduled', 'inprogress', 'finished', 'postponed', 'cancelled')"));

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