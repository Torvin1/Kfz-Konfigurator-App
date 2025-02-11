using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kfz.Database.Configuration
{
    internal class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasData(
                new Option { Id = 1, DisplayName = "Klimaanlage", Price = 2000 },
                new Option { Id = 2, DisplayName = "Alufelgen", Price = 1000 },
                new Option { Id = 3, DisplayName = "Navigation", Price = 500 },
                new Option { Id = 4, DisplayName = "Subwoofer", Price = 100 }
            );
        }
    }
}
