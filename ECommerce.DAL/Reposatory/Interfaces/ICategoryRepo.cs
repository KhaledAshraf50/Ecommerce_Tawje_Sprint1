using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface ICategoryRepo:IGenricRepo<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
    }
}
