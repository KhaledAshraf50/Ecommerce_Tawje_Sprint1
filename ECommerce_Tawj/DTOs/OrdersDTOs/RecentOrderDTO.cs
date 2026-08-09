namespace ECommerce_Tawj.DTOs.OrdersDTOs
{
    public class RecentOrderDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
