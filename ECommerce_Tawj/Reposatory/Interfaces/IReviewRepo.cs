using ECommerce_Tawj.DTOs.ReviewDTOs;
using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IReviewRepo : IGenricRepo<Review>
    {
        Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId);
        Task<bool> HasUserReviewedProductAsync(string userID, int productId);
        Task<bool> AddReviewAsync(string userID, AddReviewDTO model);
        Task<bool> UpdateReviewAsync(string userID, int reviewId, EditReviewDTO model);
        Task<bool> DeleteReviewAsync(string userID, int reviewId);

        Task<Review?> GetUserReviewForProductAsync(string userId, int productId);

    }
}
