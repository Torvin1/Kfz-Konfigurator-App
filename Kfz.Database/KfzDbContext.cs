using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Kfz.Database
{
    public class KfzDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Motor> Motors { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Order> Orders { get; set; }

        public KfzDbContext(DbContextOptions<KfzDbContext> options):base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
