using AspNetCoreGeneratedDocument;
using ECommerce_Tawj.DTOs.OrdersDTOs;
using ECommerce_Tawj.DTOs.UserDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Services.CartServices.Interfaces;
using ECommerce_Tawj.Services.EmailService;
using ECommerce_Tawj.Services.OrderServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace ECommerce_Tawj.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public OrderController(IOrderService orderService, ICartService cartService, IConfiguration configuration, IEmailService emailService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _configuration = configuration;
            _emailService = emailService;

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartByUserIdAsync(userId!);

            if (cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutDTO
            {
               Cart = cart,
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckout(CheckoutDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartByUserIdAsync(userId!);

            if (cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            model.Cart = cart;

            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            // إنشاء الأوردر
            var order = await _orderService.CreateOrderAsync(userId!, model);

            // توجيه لـ Stripe أو التفهيم اليدوي بناء على PaymentMethods Enum
            if (model.PaymentMethod == PaymentMethods.Card.ToString())
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                var stripeUrl = await _orderService.CreateStripeSessionAsync(order, domain);
                return Redirect(stripeUrl);
            }
            else
            {
                // COD (Cash On Delivery)
                return RedirectToAction(nameof(Confirmation), new { orderId = order.Id });
                   
            }

        }
        [HttpGet]
        public async Task<IActionResult> StripeSuccess(int orderId)
        {
            await _orderService.ConfirmStripePaymentAsync(orderId);
            return RedirectToAction(nameof(Confirmation), new { orderId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetOrderByIdWithUserAsync(orderId, userId!);

            if (order == null)
            {
                return NotFound();
            }

            var filePath = $"{Directory.GetCurrentDirectory()}\\EmailTemplete\\ConfirmationEmail.cshtml";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();

            str.Close();
            mailText = mailText.Replace("[OrderID]",order.Id.ToString())
                               .Replace("[UserName]", order.ShippingFirstName + " " + order.ShippingLastName)
                               .Replace("[PaymentMethod]", order.PaymentMethod)
                               .Replace("[ShippingAddress]", order.ShippingAddress + " " + order.ShippingCity)
                               .Replace("[TotalAmount]", order.TotalAmount.ToString())
                               .Replace("[OrderDate]", order.OrderDate.ToString());

            await _emailService.SendEmailAsync(order.User.Email, "Tawj Store Successful Order", mailText);

            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var orders = await _orderService.GetUserOrdersAsync(userId!);
            return View(orders);
        }

    }
}
