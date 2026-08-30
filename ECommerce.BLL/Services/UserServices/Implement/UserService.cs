using ECommerce_Tawj.DTOs.UserDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Services.UserServices.Implement
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserListDTO> GetUsersAsync(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var query = _userManager.Users.AsQueryable();
            // Filter and Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(u => u.Email!.ToLower().Contains(searchTerm) ||
                                       u.UserName!.ToLower().Contains(searchTerm));
            }
            int totalUsers = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            pageNumber = pageNumber < 1 ? 1 : pageNumber;

           if(totalPages > 0 && pageNumber > totalPages) pageNumber = totalPages;

            // Pagination 
            var usersList = await query.OrderBy(u => u.Email)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();
            var userDTO = new List<UserDTO>();

            foreach(var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var isLocked = await _userManager.IsLockedOutAsync(user);
                userDTO.Add(new UserDTO
                {
                    Id = user.Id,
                    FullName = string.IsNullOrEmpty(user.UserName) ? "N/A" : user.UserName,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "Customer",
                    IsLocked = isLocked
                });
            }

            return new UserListDTO
            {
                Users = userDTO,
                SearchTerm = searchTerm ?? "",
                PageNumber = pageNumber,
                TotalPages = totalPages
            };
        }
        public async Task ToggleRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();
            if(currentRole == "Admin")
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                if (!await _roleManager.RoleExistsAsync("Customer"))
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));

                await _userManager.AddToRoleAsync(user, "Customer");
            }
            else
            {
                if (currentRole != null)
                    await _userManager.RemoveFromRoleAsync(user, currentRole);

                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        public async Task ToggleLockStatusAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {

                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                // lock to Two Years
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(2));
            }

        }

    }
}
