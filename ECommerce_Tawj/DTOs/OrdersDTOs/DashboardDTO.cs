namespace ECommerce_Tawj.DTOs.OrdersDTOs
{
    public class DashboardDTO
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveUsers { get; set; }
        public IEnumerable<RecentOrderDTO> RecentOrders { get; set; } = new List<RecentOrderDTO>();
    }
}
