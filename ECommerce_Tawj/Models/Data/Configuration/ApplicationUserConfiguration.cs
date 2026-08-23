using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Tawj.Models.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Full Name
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            // Created At
            builder.Property(u => u.CreatedAt)
                .IsRequired();

            // Is Locked
            builder.Property(u => u.IsLocked)
                .IsRequired()
                .HasDefaultValue(false);

            // Address
            builder.Property(u => u.Address)
                .IsRequired();

            // User -> Orders
            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> CartItems
            builder.HasMany(u => u.CartItems)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
