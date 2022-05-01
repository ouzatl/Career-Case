using Career.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Career.Data
{
    public class CareerContext : DbContext
    {
        public CareerContext()
        {
        }
        public CareerContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("A FALLBACK CONNECTION STRING");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<BannedWords> BannedWords { get; set; }
    }
}