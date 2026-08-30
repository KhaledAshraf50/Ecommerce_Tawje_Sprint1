using ECommerce_Tawj.Services.CartServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ECommerce_Tawj.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServiceSession _cartServiceSession;

        private readonly ICartService _cartService;

        public CartController
            (ICartService cartService,
             ICartServiceSession cartServiceSession)
        {
            _cartServiceSession = cartServiceSession;
            _cartService = cartService;
        }
        private string? GetUserId()
        {
            return User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
        }
        //public IActionResult? IsLogin(string userId)
        //{
        //    if (userId == null)
        //    {
        //        var cart = _cartServiceSession.GetCart();
        //        return View("Index",cart);
        //    }
        //    return null;
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            // Guest User -> Session Cart
            if (userId == null)
            {
                var sessionCart = _cartServiceSession.GetCart();

                return View("SessionCart", sessionCart);
            }
            // Logged In User -> Database Cart
            var dbCart = await _cartService.GetCartByUserIdAsync(userId);

            return View("Index", dbCart);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                // Guest -> Session
                await _cartServiceSession.AddItemAsync(productId,quantity);
            }
            else
            {
                await _cartService.AddToCartAsync(userId,productId,quantity);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = GetUserId();
            await _cartService.UpdateQuantityAsync(userId!, cartItemId, quantity);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {

                // Guest -> Session
                await _cartServiceSession
                    .IncreaseQuantityAsync(productId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            await _cartServiceSession
                     .DecreaseQuantityAsync(productId);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                // Guest -> Session
                _cartServiceSession.RemoveItem(productId);
            }
            else
            {
                await _cartService.RemoveProductFromCartAsync(userId, productId);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                // Guest -> Session
                _cartServiceSession.ClearCart();
            }
            else
            {
                // Logged In -> Database

                await _cartService.ClearCartAsync(userId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
