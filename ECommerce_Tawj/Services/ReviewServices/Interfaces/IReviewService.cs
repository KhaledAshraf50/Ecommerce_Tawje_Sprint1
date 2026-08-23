using ECommerce_Tawj.DTOs.ReviewDTOs;

namespace ECommerce_Tawj.Services.ReviewServices.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId);
        Task<bool> AddReviewAsync(string userID, AddReviewDTO model);
        Task<bool> UpdateReviewAsync(string userID, int reviewId, EditReviewDTO model);
        Task<bool> DeleteReviewAsync(string userID, int reviewId);

        Task<bool> HasUserReviewedProductAsync(string userID, int productId);
    }
}
