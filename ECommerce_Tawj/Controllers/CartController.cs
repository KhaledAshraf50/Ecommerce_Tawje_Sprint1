using ECommerce_Tawj.Services.CartServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ECommerce_Tawj.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServiceSession _cartServiceSession;

        //private readonly ICartService _cartService;

        public CartController
            (/*ICartService cartService*/
             ICartServiceSession cartServiceSession)
        {
            _cartServiceSession = cartServiceSession;
            //_cartService = cartService;
        }
        //public string GetUserId()
        //{
        //    return User.Identity.IsAuthenticated ? 
        //        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null;
        //}
        //public  IActionResult? IsLogin(string userId)
        //{
        //    if (userId == null)
        //    {
        //        return  RedirectToAction("Login", "Account");
        //    }
        //    return null;
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var userId = GetUserId();
            //var loginCheck = IsLogin(userId);
            //if(loginCheck != null) return loginCheck;

            //var cart = await _cartService.GetCartByUserIdAsync(userId);
            var cart = _cartServiceSession.GetCart();
  
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            //var userId = GetUserId();
            //var loginCheck = IsLogin(userId);
            //if (loginCheck != null) return loginCheck;
            //await _cartService.AddToCartAsync(userId!, productId, quantity);
            await _cartServiceSession.AddItemAsync(productId, quantity);
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        //{
        //    //var userId = GetUserId();
        //    //var loginCheck = IsLogin(userId);
        //    //if (loginCheck != null) return loginCheck;
        //    //await _cartService.UpdateQuantityAsync(userId!, cartItemId, quantity);

        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            await _cartServiceSession.IncreaseQuantityAsync(productId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            await _cartServiceSession.DecreaseQuantityAsync(productId);

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            //var userId = GetUserId();
            //var loginCheck = IsLogin(userId);
            //if (loginCheck != null) return loginCheck;
            //await _cartService.RemoveFromCartAsync(userId!, cartItemId);
            _cartServiceSession.RemoveItem(productId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Clear()
        {
            _cartServiceSession.ClearCart();

            return RedirectToAction(nameof(Index));
        }
    }
}
