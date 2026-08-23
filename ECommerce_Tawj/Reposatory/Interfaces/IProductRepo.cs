using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IProductRepo:IGenricRepo<Product>
    {
        Task<IEnumerable<Product>> GetProductWithCategoriesWithProImages();
        Task<Product?> GetProductWithDetailsByIdAsync(int id);

        IQueryable<Product> GetAllQueryable();
        Task<Product?> GetProductWithDetailsForUpdateAsync(int id);
        Task<Product?> GetDeletedProductByIdAsync(int id);

        Task<IEnumerable<Product>> GetDeletedProductsAsync();

    }
}
