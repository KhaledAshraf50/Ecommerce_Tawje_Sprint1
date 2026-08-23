using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Models.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Review { get; set; }
        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configurations can be added here if needed
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            ApplyAuditChanges();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditChanges();

            return await base.SaveChangesAsync(
                acceptAllChangesOnSuccess,
                cancellationToken);
        }
        private void ApplyAuditChanges()
        {
            var entries = ChangeTracker
                .Entries<IAuditableEntity>();

            foreach (var entity in entries)
            {
                switch (entity.State)
                {
                    case EntityState.Added:

                        entity.Entity.CreatedAt = DateTime.UtcNow;
                        entity.Entity.UpdatedAt = DateTime.UtcNow;

                        break;

                    case EntityState.Modified:

                        entity.Entity.UpdatedAt = DateTime.UtcNow;

                        break;

                    case EntityState.Deleted:

                        entity.State = EntityState.Modified;
                        entity.Entity.IsDeleted = true;
                        entity.Entity.UpdatedAt = DateTime.UtcNow;

                        break;
                }
            }
        }
    }
}
