using ECommerce_Tawj.DTOs.CartItemDTOs;
using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Services.CartServices.Interfaces
{
    public interface ICartServiceSession
    {
        Task AddItemAsync(int productId, int quantity = 1);
        List<SessionCartItem> GetCart();

        //Task UpdateQuantityAsync(int productId, int quantity);
        Task IncreaseQuantityAsync(int productId);
        Task DecreaseQuantityAsync(int productId);
        void RemoveItem(int productId);
        void ClearCart();
        int GetCartCount();

    }
}
