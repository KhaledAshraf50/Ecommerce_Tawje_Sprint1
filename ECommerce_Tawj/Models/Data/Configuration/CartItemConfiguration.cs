using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            // Quantity
            builder.Property(c => c.Quantity)
                .IsRequired();

            // User -> CartItems
            builder.HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> CartItems
            builder.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // One user can't have the same product twice

            builder.HasIndex(c => new
            {
                c.ProductId,
                c.UserId
            })
                .IsUnique();
        }
    }
}
