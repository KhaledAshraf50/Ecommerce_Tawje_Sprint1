using ECommerce_Tawj.DTOs.AccountDTOs;
using ECommerce_Tawj.Services.AccountServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
           _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(userDTO);
            }
            var result = await _accountService.RegisterAsync(userDTO);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(userDTO);  
        }
        // Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            if(!ModelState.IsValid) return View(userDTO);
            var result = await _accountService.LoginAsync(userDTO);
            if (result.Succeeded) 
                return RedirectToAction("Index", "Home");
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "This account has been locked. Please try again later.");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Email Or Password");
            }
            return View(userDTO);
        }
        // Logout
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
