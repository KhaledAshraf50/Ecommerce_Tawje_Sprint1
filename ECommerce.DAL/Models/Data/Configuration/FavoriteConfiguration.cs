using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            // Primary Key
            builder.HasKey(f => f.Id);

            // Created At
            builder.Property(f => f.CreatedAt)
                .IsRequired();

            // User -> Favorites
            builder.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Product)
              .WithMany()
              .HasForeignKey(f => f.ProductId)
              .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate favorite
            builder.HasIndex(f => new
            {
                f.UserId,
                f.ProductId
            })
                .IsUnique();
        }
    }
}
