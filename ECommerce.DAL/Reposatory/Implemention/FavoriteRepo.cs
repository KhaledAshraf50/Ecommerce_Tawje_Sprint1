using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class FavoriteRepo : GenricRepo<Favorite>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesWithProductsAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .ThenInclude(p=>p.Images)
                .ToListAsync();
        }
        public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId)
        {
            return await _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
