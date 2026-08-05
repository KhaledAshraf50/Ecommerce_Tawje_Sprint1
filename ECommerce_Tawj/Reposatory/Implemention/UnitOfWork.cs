using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepo ProductRepo { get; private set; }
        public IGenricRepo<ProductImage> ProductImageRepo { get; private set; }
        public ICategoryRepo CategoryRepo { get; private set; }
        public IFavoriteRepo FavoriteRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepo = new ProductRepo(_context);
            ProductImageRepo = new GenricRepo<ProductImage>(_context);
            CategoryRepo = new CategoryRepo(_context);
            FavoriteRepo = new FavoriteRepo(_context);
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
