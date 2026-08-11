using ECommerce_Tawj.DTOs.HomeDTOs;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Services.ProductServices.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(CreateProductDTO productDto);
        Task<IEnumerable<Product>> GetProductWithCategoriesWithProImagesAsync();
        Task<HomeDTO> GetHomePageDataAsync(string? userId);

        Task<ProductDetailsDTO?> GetProductDetailsByIdAsync(int productId);

        Task<ShopDTO> GetShopProductsAsync
            (string? searchTerm, int? categoryId, string? userId, int pageNumber = 1, int pageSize = 9);

        Task<ProductEditDTO?> GetProductForEditAsync(int id);
        Task UpdateProductAsync(ProductEditDTO model);
        Task<bool> DeleteProductAsync(int id);
    }
}
