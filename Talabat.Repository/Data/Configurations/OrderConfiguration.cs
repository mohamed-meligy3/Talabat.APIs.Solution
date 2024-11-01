using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShipToAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(O => O.Status)
                   .HasConversion(
                     OS => OS.ToString(),
                     OS => (OrderState)Enum.Parse(typeof(OrderState), OS));

            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
