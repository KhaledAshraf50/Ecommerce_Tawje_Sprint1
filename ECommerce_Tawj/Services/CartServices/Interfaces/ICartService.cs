using ECommerce_Tawj.DTOs.CartItemDTOs;

namespace ECommerce_Tawj.Services.CartServices.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId, int quantity = 1);
        Task<CartDTO> GetCartByUserIdAsync(string userId);
        Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(string userId, int cartItemId);

        Task<int> GetCartCountAsync(string userId);

        // Increase Quantity to Increase Quantity
        
        // Decrease Quantity to Decrease Quantity

        // Clear Cart to empty the cart entirely

    }
}
