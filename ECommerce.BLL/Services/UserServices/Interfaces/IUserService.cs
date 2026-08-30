using ECommerce_Tawj.DTOs.UserDTOs;

namespace ECommerce_Tawj.Services.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<UserListDTO> GetUsersAsync(string? searchTerm, int pageNumber = 1, int pageSize = 5);
        Task ToggleRoleAsync(string userId);
        Task ToggleLockStatusAsync(string userId);
    }
}
