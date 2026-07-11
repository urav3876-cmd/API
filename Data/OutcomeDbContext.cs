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
                entity.ToTable("outcomes");

                entity.HasKey(e => e.OutcomeId);

                entity.Property(e => e.OutcomeId)
                    .HasColumnName("outcome_id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .IsRequired();

                entity.Property(e => e.OutcomeName)
                    .HasColumnName("outcome_name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OutcomeScore)
                    .HasColumnName("outcome_score")
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .IsRequired();

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValue(false);

                entity.HasOne(o => o.Event)
                    .WithMany(e => e.Outcomes)
                    .HasForeignKey(o => o.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}