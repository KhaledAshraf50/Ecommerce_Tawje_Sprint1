using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class CartRepo : GenricRepo<CartItem>, ICartRepo
    {
        public CartRepo(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                    .Where(c => c.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
