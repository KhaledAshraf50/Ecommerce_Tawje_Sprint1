using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // Order Date
            builder.Property(o => o.OrderDate)
                .IsRequired();

            // Total Amount
            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Status
            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Payment Method
            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            // Shipping Information
            builder.Property(o => o.ShippingFirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.ShippingLastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(o => o.ShippingCity)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.ShippingPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
