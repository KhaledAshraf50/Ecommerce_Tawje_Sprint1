using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            //pk
            builder.HasKey(r => r.Id);

            //rating
            builder.Property(r => r.Rating)
                .HasDefaultValue(5);

            //rating
            builder.Property(r => r.Comment)
                .HasMaxLength(200);

            //timestamp
            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .IsRequired();

            //rel
            builder.HasOne(r=>r.Product)
                .WithMany()
                .HasForeignKey(r=> r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r=>r.User)
                .WithMany()
                .HasForeignKey(r=>r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new
            {
                r.ProductId,
                r.UserId
            })
                .IsUnique();

            // Global Query
            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
