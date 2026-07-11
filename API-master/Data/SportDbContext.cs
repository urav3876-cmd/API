using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class SportDbContext : DbContext
    {
        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }

        // ============ ТАБЛИЦЫ ============
        public DbSet<Sport> Sports { get; set; }

        // ============ КОНФИГУРАЦИЯ ============
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.ToTable("sports");

                entity.HasKey(e => e.SportId);

                entity.Property(e => e.SportId)
                    .HasColumnName("sport_id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.SportName)
                    .HasColumnName("sport_name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Глобальный фильтр (исключаем удаленные)
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}