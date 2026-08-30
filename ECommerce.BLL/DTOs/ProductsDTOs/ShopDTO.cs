using ECommerce_Tawj.DTOs.CategoryDTOs;

namespace ECommerce_Tawj.DTOs.ProductsDTOs
{
    public class ShopDTO
    {
        public IEnumerable<ProductCardDTO> Products { get; set; } = new List<ProductCardDTO>();
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public int? SelectedCategoryId { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? SortOrder { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
