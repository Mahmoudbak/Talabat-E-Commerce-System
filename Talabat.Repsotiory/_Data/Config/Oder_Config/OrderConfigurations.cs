using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Order_Aggreate;

namespace Talabat.Repsotiory._Data.Config.Oder_Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(order => order.Status)
                .HasConversion
                (   
                 (OStutas) => OStutas.ToString(),
                 (OStutas)=>(OrderStatus) Enum.Parse(typeof(OrderStatus),OStutas)
                );
            #region This is one to one Acul

            //builder.HasOne(order => order.DeliveryMethod)
            //    .WithOne();
            //builder.HasIndex("DeliveryMethodId").IsUnique(True); 
            #endregion


            builder.Property(order => order.SubTotal)
                .HasColumnType("decimal(12,2)");

            builder.HasOne(order => order.DeliveryMethod)
               .WithMany()
               .OnDelete(DeleteBehavior.SetNull);


            builder.HasMany(order=>order.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
