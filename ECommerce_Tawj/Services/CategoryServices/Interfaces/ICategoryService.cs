using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Services.CategoryServices.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task AddCategoryAsync(AddCategoryDTO model);
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(CategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
