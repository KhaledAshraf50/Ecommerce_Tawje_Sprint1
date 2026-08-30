using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class ProductRepo : GenricRepo<Product>, IProductRepo
    {
        public ProductRepo(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Product> GetAllQueryable()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p=>p.Images)
                .AsNoTracking(); // للأداء العالي مع القراءة فقط
        }

        public async Task<IEnumerable<Product>> GetProductWithCategoriesWithProImages()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product?> GetProductWithDetailsForUpdateAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
