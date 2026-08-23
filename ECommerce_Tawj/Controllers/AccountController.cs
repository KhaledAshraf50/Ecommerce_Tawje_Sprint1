using ECommerce_Tawj.DTOs.AccountDTOs;
using ECommerce_Tawj.Services.AccountServices.Interfaces;
using ECommerce_Tawj.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ECommerce_Tawj.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        public AccountController(
            IAccountService accountService,
            IEmailService emailService)
        {
           _accountService = accountService;
           _emailService = emailService;

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
                var filePath = $"{Directory.GetCurrentDirectory()}\\EmailTemplete\\WelcomeEmail.cshtml";
                var str = new StreamReader(filePath);

                var mailText = str.ReadToEnd();

                str.Close();
                mailText = mailText.Replace("[UserName]", userDTO.fullName);
                await _emailService.SendEmailAsync(userDTO.Email,"Tawj Store Successful Registration", mailText);
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
