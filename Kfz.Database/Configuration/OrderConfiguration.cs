using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kfz.Database.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(or => or.Id);
            builder.HasOne(or => or.Manufacturer).WithMany(m => m.Orders).HasForeignKey(or => or.ManufacturerId).OnDelete(DeleteBehavior.Restrict).IsRequired(true);
            builder.HasOne(or => or.Motor).WithMany(mo => mo.Orders).HasForeignKey(or => or.MotorId).OnDelete(DeleteBehavior.Restrict).IsRequired(true);
            builder.HasMany(or => or.Options).WithMany(mo => mo.Orders).UsingEntity("OrderOptions");
        }
    }
}
