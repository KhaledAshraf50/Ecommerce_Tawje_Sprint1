using ECommerce_Tawj.Services.AdminServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            var dashboardData = await _adminService.GetDashboardDataAsync();
            return View(dashboardData);
        }
    }
}
