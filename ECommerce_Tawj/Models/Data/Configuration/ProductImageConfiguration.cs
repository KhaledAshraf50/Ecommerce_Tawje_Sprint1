using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            // pk
            builder.HasKey(pi => pi.Id);

            //imageUrl
            builder.Property(pi => pi.ImageUrl)
                .IsRequired();

            //Product Rel
            builder.HasOne(pi => pi.Product)
                .WithMany(P=>P.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
