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

        public DbSet<Sport> Sports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}