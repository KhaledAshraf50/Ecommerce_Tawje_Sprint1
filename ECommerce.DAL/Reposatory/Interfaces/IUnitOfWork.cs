using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepo ProductRepo { get; }
        IGenricRepo<ProductImage> ProductImageRepo { get; }
        ICategoryRepo CategoryRepo { get; }
        IFavoriteRepo FavoriteRepo { get; }
        ICartRepo CartRepo { get; }
        IOrderRepo OrderRepo { get; }
        IReviewRepo ReviewRepo { get; }
        Task<int> SaveChangesAsync();
    }
}
