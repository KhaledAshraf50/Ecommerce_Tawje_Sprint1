namespace ECommerce_Tawj.DTOs.OrdersDTOs
{
    public class UserOrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int ItemsCount { get; set; }
        public string ItemsSummary { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
