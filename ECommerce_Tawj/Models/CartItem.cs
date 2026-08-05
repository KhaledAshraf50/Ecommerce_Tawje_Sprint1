using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce_Tawj.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; } = 1;

        // Foreign Keys
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
