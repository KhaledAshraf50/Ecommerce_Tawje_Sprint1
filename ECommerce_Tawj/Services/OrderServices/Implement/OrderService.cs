using AutoMapper;
using ECommerce_Tawj.DTOs.OrdersDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CartServices.Interfaces;
using ECommerce_Tawj.Services.OrderServices.Interfaces;
using Stripe.Checkout;

namespace ECommerce_Tawj.Services.OrderServices.Implement
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AdminOrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.OrderRepo.GetAllOrdersWithDetailsAsync();
            return _mapper.Map<IEnumerable<AdminOrderDTO>>(orders);
        }
        public async Task<Order> CreateOrderAsync(string userId, CheckoutDTO model)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart.Items == null || !cart.Items.Any())
                throw new Exception("Cart is empty.");

            model.Cart = cart;

            var order = _mapper.Map<Order>(model);

            order.UserId = userId;
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = cart.GrandTotal;
            order.Status = OrderStatus.Pending.ToString();

            _unitOfWork.OrderRepo.Add(order);
            await _unitOfWork.SaveChangesAsync();

            // تفريغ السلة
            var userCartItems = await _unitOfWork.CartRepo.GetCartItemsByUserIdAsync(userId);
            foreach (var item in userCartItems)
            {
                await _unitOfWork.CartRepo.DeleteAsync(item.Id);
            }
            await _unitOfWork.SaveChangesAsync();

            // إعادة الأوردر شاملاً تفاصيل المنتجات
            var createdOrder = await _unitOfWork.OrderRepo.GetOrderWithItemsByIdAsync(order.Id, userId);
            return createdOrder ?? order;
        }
        public async Task<string> CreateStripeSessionAsync(Order order, string domain)
        {
            // التأكد من جلب المنتجات إذا لم تكن محملة
            if (order.OrderItems.Any(i => i.Product == null))
            {
                var fullOrder = await _unitOfWork.OrderRepo.GetOrderWithItemsByIdAsync(order.Id, order.UserId);
                if (fullOrder != null)
                {
                    order = fullOrder;
                }
            }
            var lineItems = new List<SessionLineItemOptions>();
            foreach(var item in order.OrderItems)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = item.UnitPrice * 100,
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        }
                    },
                    Quantity = item.Quantity
                });
            }
            // مصاريف الشحن 
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = 50 * 100, // مصاريف الشحن
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shipping Fee",
                    }
                },
                Quantity = 1
            });
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{domain}/order/StripeSuccess?orderId={order.Id}",
                CancelUrl = $"{domain}/order/Checkout"
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }
        public async Task ConfirmStripePaymentAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Processing.ToString();
                _unitOfWork.OrderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }



        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _unitOfWork.OrderRepo.GetOrderWithItemsByIdAsync(orderId, userId);
        }
        public async Task<Order?> GetOrderByIdWithUserAsync(int orderId, string userId)
        {
            return await _unitOfWork.OrderRepo.GetOrderWithItemsByIdWithUserAsync(orderId, userId);
        }

        public async Task<IEnumerable<UserOrderDTO>> GetUserOrdersAsync(string userId)
        {
            var userOrders = await _unitOfWork.OrderRepo.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserOrderDTO>>(userOrders);
        }
    }
}
