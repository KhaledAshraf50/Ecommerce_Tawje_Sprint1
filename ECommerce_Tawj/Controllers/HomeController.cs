using ECommerce_Tawj.Models;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce_Tawj.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.IsAuthenticated ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null;
            var homeData = await _productService.GetHomePageDataAsync(userId);
            return View(homeData);
        }

    }
}
