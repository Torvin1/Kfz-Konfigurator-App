using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kfz.Database.Configuration
{
    internal class FuelConfiguration : IEntityTypeConfiguration<Fuel>
    {
        public void Configure(EntityTypeBuilder<Fuel> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasData(
                new Fuel { Id = 1, DisplayName = "Elektro", EcoFriendlinessRating = 1 },
                new Fuel { Id = 2, DisplayName = "Hybrid", EcoFriendlinessRating = 2 },
                new Fuel { Id = 3, DisplayName = "Benzin", EcoFriendlinessRating = 3 },
                new Fuel { Id = 4, DisplayName = "Diesel", EcoFriendlinessRating = 4 }
            );
        }
    }
}
