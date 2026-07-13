using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class OutcomeDbContext : DbContext
    {
        public OutcomeDbContext(DbContextOptions<OutcomeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Outcome> Outcomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Outcome>(entity =>
            {
                entity.HasOne(o => o.Event)
                    .WithMany(e => e.Outcomes)
                    .HasForeignKey(o => o.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}