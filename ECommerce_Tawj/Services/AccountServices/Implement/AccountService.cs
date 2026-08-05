using AutoMapper;
using ECommerce_Tawj.DTOs.AccountDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Services.AccountServices.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Tawj.Services.AccountServices.Implement
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly IMapper _mapper;

        public AccountService
            (UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDTO registerDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            return result;
        }
        public Task<SignInResult> LoginAsync(LoginUserDTO loginDto)
        {
            var result = _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                loginDto.RememberMe,
                lockoutOnFailure:true);// if user failed to login 5 times the account will be locked for 5 minutes
            return result;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
