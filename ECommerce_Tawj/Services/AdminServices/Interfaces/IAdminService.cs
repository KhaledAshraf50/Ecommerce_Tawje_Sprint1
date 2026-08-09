using ECommerce_Tawj.DTOs.OrdersDTOs;

namespace ECommerce_Tawj.Services.AdminServices.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardDTO> GetDashboardDataAsync();
    }
}
