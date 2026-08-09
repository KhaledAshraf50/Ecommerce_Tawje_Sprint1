using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.DTOs.ProductsDTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "السعر مطلوب")]
        public decimal Price { get; set; }
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public int DiscountPercentage { get; set; }
        [Required(ErrorMessage = "يجب اختيار فئة للمنتج")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="يجب اختيار صورة واحده علي الاقل!")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
