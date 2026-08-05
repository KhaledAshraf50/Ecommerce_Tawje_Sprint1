using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce_Tawj.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
    public enum PaymentMethods
    {
        COD,
        Card
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50,ErrorMessage = "Status is required.")]
        public string Status { get; set; } = OrderStatus.Pending.ToString(); // Pending, Processing, Completed, Cancelled

        [Required]
        [MaxLength(50,ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = PaymentMethods.COD.ToString(); // COD, Card
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100,ErrorMessage = "First name cannot exceed 100 characters.")]
        public string ShippingFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100,ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string ShippingLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250,ErrorMessage = "Address cannot exceed 250 characters.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100,ErrorMessage = "City cannot exceed 100 characters.")]
        public string ShippingCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(20,ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string ShippingPhone { get; set; } = string.Empty;
        // Foreign Key
        [Required(ErrorMessage = "User is required.")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        // Navigation Property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
