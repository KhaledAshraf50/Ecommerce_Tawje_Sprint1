namespace ECommerce_Tawj.DTOs.CartItemDTOs
{
    public class CartDTO
    {
        public IEnumerable<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal SubTotal => Items.Sum(i => i.TotalPrice);
        public decimal ShippingFee => Items.Any() ? 15.00m : 0.00m;
        public decimal GrandTotal => SubTotal + ShippingFee;
    }
}
