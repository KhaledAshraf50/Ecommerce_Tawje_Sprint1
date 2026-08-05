using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.DTOs.CategoryDTOs
{
    public class AddCategoryDTO
    {
        [Required(ErrorMessage = "اسم الفئة مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
