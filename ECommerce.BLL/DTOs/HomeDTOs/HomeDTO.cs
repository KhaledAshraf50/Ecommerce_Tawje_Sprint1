using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.ViewModels.ProductsVM;

namespace ECommerce_Tawj.DTOs.HomeDTOs
{
    public class HomeDTO
    {
        public IEnumerable<ProductsHomeDTO> HeroDeals { get; set; } = new List<ProductsHomeDTO>();
        public IEnumerable<ProductsHomeDTO> PopularProducts { get; set; } = new List<ProductsHomeDTO>();
        public IEnumerable<ProductsHomeDTO> FeaturedProducts { get; set; } = new List<ProductsHomeDTO>();
    }
}
