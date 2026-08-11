using ECommerce_Tawj.DTOs.CategoryDTOs;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.DTOs.ProductsDTOs
{
    public class ProductEditDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        public string? ExistingImageUrl { get; set; }
        public IFormFile? NewImage { get; set; }

        public IEnumerable<CategoryDTO>? Categories { get; set; }
    }
}
