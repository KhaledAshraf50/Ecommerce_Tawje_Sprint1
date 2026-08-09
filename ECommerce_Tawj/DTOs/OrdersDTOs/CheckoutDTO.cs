using ECommerce_Tawj.DTOs.CartItemDTOs;
using ECommerce_Tawj.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.DTOs.OrdersDTOs
{
    public class CheckoutDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string ShippingFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string ShippingLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string ShippingCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string ShippingPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = PaymentMethods.COD.ToString();

        public CartDTO? Cart { get; set; }
    }
}
