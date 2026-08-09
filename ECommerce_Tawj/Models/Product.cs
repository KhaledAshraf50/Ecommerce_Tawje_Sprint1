using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce_Tawj.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200,ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; }=string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]// Specify the precision and scale for the Price property
        public decimal Price { get; set; }
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public int DiscountPercentage { get; set; } = 0;

        public int StockQuantity { get; set; } = 0;
        [MaxLength(100,ErrorMessage = "Brand cannot exceed 100 characters.")]
        public string Brand { get; set; } = string.Empty;
        [Range(0, 5)]
        public double AverageRating { get; set; } = 0.0;
        // Foreign Key
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        // Navigation Properties
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    }
}
