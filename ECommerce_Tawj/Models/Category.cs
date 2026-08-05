using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100,ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500,ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Navigation property for the related Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
