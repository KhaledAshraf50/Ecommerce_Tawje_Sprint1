using ECommerce_Tawj.DTOs.ProductsDTOs;

namespace ECommerce_Tawj.Services.FavoriteService.Interface
{
    public interface IFavoriteService
    {
        Task<bool> ToggleFavoriteAsync(string userId, int productId);
        Task<IEnumerable<ProductsHomeDTO>> GetUserFavoritesAsync(string userId);
        Task<List<int>> GetUserFavoriteProductIdsAsync(string userId);
    }
}
