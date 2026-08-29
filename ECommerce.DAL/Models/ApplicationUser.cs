using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100,ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsLocked { get; set; }=false;
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        // Navigation property for the related Cart entity

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
