using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface ICartRepo :IGenricRepo<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId);
    }
}
