using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce_Tawj.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } = false;
        // Foreign Key
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
