using ECommerce_Tawj.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm,int pageNumber = 1)
        {
            var model = await _userService.GetUsersAsync(searchTerm, pageNumber);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleRole(string userId, string? searchTerm, int pageNumber = 1)
        {
            await _userService.ToggleRoleAsync(userId);
            return RedirectToAction("Index", new { searchTerm, pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ToggleLock(string userId, string? searchTerm, int pageNumber = 1)
        {
            await _userService.ToggleLockStatusAsync(userId);
            return RedirectToAction("Index", new { searchTerm, pageNumber });
        }
    }
}
