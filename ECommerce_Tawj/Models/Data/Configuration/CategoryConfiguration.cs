using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // pk
            builder.HasKey(c => c.Id);

            // NAme
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            //Description
            builder.Property(c => c.Description)
                .HasMaxLength(500);

            // product Rel
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Restrict);

            //IsDeleted
            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // timeStamp

            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.Property(c=>c.UpdatedAt)
                .IsRequired();

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
