using ECommerce_Tawj.DTOs.OrdersDTOs;
using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Services.OrderServices.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<AdminOrderDTO>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(string userId, CheckoutDTO model);
        Task<string> CreateStripeSessionAsync(Order order, string domain);

        Task ConfirmStripePaymentAsync(int orderId);
        Task<Order?> GetOrderByIdAsync(int orderId, string userId);

        Task<IEnumerable<UserOrderDTO>> GetUserOrdersAsync(string userId);
    }
}
