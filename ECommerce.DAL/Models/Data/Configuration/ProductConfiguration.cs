using ECommerce_Tawj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // Name
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Description
            builder.Property(p => p.Description)
                .IsRequired();

            // Price
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Discount
            builder.Property(p => p.DiscountPercentage)
                .IsRequired()
                .HasDefaultValue(0);

            // Stock
            builder.Property(p => p.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            // Brand
            builder.Property(p => p.Brand)
                .HasMaxLength(100);

            // Average Rating
            builder.Property(p => p.AverageRating)
                .IsRequired()
                .HasDefaultValue(0.0);

            // Category relationship
            builder.HasOne(p=>p.Category)
                .WithMany()
                .HasForeignKey(p=>p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // soft delete

            builder.Property(p => p.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

            // Timestamps
            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired();

            // Global Query Filter
            builder.HasQueryFilter(p => !p.IsDeleted); // context.products.tolistasync() =>  NOT include products with Isdeleted 
            // to access it use => context.products.IgnoreQueryFilter().tolistasync()

        }
    }
}