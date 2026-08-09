using AutoMapper;
using ECommerce_Tawj.DTOs.OrdersDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.AdminServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Services.AdminServices.Implement
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            var orders = await _unitOfWork.OrderRepo.GetAllOrdersWithDetailsAsync();
            var products = await _unitOfWork.ProductRepo.GetAllAsync(null);
            var totalUsersCount = await _userManager.Users.CountAsync();

            // 2. حساب أحدث 5 طلبات
            var recentOrdersEntities = orders.Take(5);

            return new DashboardDTO
            {
                TotalSales = orders.Sum(o => o.TotalAmount),
                TotalOrders = orders.Count(),
                TotalProducts = products.Count(),
                ActiveUsers = totalUsersCount,
                RecentOrders = _mapper.Map<IEnumerable<RecentOrderDTO>>(recentOrdersEntities)
            };
        }
    }
}
