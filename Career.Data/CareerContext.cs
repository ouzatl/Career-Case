using Career.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Career.Data
{
    public class CareerContext : DbContext
    {
        public CareerContext(DbContextOptions<CareerContext> options) : base(options)
        {

        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}