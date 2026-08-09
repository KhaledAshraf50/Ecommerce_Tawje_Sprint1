namespace ECommerce_Tawj.DTOs.OrdersDTOs
{
    public class AdminOrderDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingPhone { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int ItemsCount { get; set; }

        // نص يحتوي على أسماء المنتجات والكميات (مثل: AirPods Pro x 1, MacBook Pro x 2)
        public string ItemsSummary { get; set; } = string.Empty;
    }
}
