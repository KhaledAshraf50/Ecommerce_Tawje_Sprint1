using ECommerce_Tawj.Models;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IReviewRepo : IGenricRepo<Review>
    {
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task<bool> HasUserReviewedProductAsync(string userID, int productId);
        Task<bool> AddReviewAsync(string userID, Review model);
        Task<bool> UpdateReviewAsync(string userID, int reviewId, Review model);
        Task<bool> DeleteReviewAsync(string userID, int reviewId);

        Task<Review?> GetUserReviewForProductAsync(string userId, int productId);

    }
}
