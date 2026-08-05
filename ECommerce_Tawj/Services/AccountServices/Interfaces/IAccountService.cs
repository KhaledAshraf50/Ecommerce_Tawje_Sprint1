using ECommerce_Tawj.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Tawj.Services.AccountServices.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDTO registerDto);
        Task<SignInResult> LoginAsync(LoginUserDTO loginDto);
        Task LogoutAsync();
    }
}
