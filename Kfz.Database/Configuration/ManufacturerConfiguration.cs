using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kfz.Database.Configuration
{
    internal class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasData(
                new Manufacturer { Id = 1, DisplayName = "Volkswagen", BasePrice = 15000 },
                new Manufacturer { Id = 2, DisplayName = "Opel", BasePrice = 20000 },
                new Manufacturer { Id = 3, DisplayName = "BMW", BasePrice = 30000 }
            );
        }
    }
}
