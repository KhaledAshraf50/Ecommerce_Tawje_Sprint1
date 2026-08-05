using ECommerce_Tawj.Services.FavoriteService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce_Tawj.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            bool isFavorite = await _favoriteService.ToggleFavoriteAsync(userId, productId);
            return Json(new { success = true, isFavorite = isFavorite });
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favoriteService.GetUserFavoritesAsync(userId!);
            return View(favorites);
        }
    }
}
