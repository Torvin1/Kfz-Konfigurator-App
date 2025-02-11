using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kfz.Database.Configuration
{
    internal class MotorConfiguration : IEntityTypeConfiguration<Motor>
    {
        public void Configure(EntityTypeBuilder<Motor> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasOne(m => m.Fuel).WithMany(f => f.Motors).HasForeignKey(m => m.FuelId).IsRequired(true);

            builder.HasData(
                new Motor { Id = 1, DisplayName = "Benzin 1.6", Price = 1500, FuelId = 3 },
                new Motor { Id = 2, DisplayName = "Benzin 2.5", Price = 5000, FuelId = 3 },
                new Motor { Id = 3, DisplayName = "Diesel 1.8", Price = 2000, FuelId = 4 },
                new Motor { Id = 4, DisplayName = "Diesel 3.5", Price = 4000, FuelId = 4 },
                new Motor { Id = 5, DisplayName = "Hybrid 1.0", Price = 2500, FuelId = 2 },
                new Motor { Id = 6, DisplayName = "Hybrid 2.2", Price = 3500, FuelId = 2 },
                new Motor { Id = 7, DisplayName = "Elektro 1.0", Price = 4500, FuelId = 1 },
                new Motor { Id = 8, DisplayName = "Elektro 1.5", Price = 6000, FuelId = 1 }
            );
        }
    }
}
