using ECommerce_Tawj.Models;
using ECommerce_Tawj.Services.EmailService;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerce_Tawj.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;

        public HomeController(IProductService productService,IEmailService emailService)
        {
            _productService = productService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
            var homeData = await _productService.GetHomePageDataAsync(userId);
            return View(homeData);
        }
        public async Task<IActionResult> TestEmail()
        {
            await _emailService.SendEmailAsync(
                "test@example.com",
                "Tawj Test Email",
                """
                <h1>Hello From Tawj</h1>
                <p>This is a test email.</p>
                """
                );
            return Content("Email Sent Successfully");
        }

    }
}
