namespace ECommerce_Tawj.DTOs.CartItemDTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string MainImage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice => ProductPrice * Quantity;
    }
}
