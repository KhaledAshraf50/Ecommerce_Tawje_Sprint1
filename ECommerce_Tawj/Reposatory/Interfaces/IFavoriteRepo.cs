using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IFavoriteRepo : IGenricRepo<Favorite>
    {
        Task<IEnumerable<Favorite>> GetFavoritesWithProductsAsync(string userId);
    }
}
