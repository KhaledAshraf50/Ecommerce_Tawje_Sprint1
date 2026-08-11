using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IOrderRepo : IGenricRepo<Order>
    {
       Task<Order?> GetOrderWithItemsByIdAsync(int orderId, string userId);
        Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
