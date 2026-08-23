using ECommerce_Tawj.DTOs.ReviewDTOs;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.ReviewServices.Interfaces;

namespace ECommerce_Tawj.Services.ReviewServices.Implement
{
    public class ReviewsService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId)
        {
            return await _unitOfWork.ReviewRepo.GetProductReviewsAsync(productId);
        }
        public async Task<bool> AddReviewAsync(string userID, AddReviewDTO model)
        {
            var result = await _unitOfWork.ReviewRepo.AddReviewAsync(userID, model);
            if (!result) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReviewAsync(string userID, int reviewId)
        {
            var result = await _unitOfWork.ReviewRepo.DeleteReviewAsync(userID, reviewId);
            if (!result) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public async Task<bool> HasUserReviewedProductAsync(string userID, int productId)
        {
            return await _unitOfWork.ReviewRepo.HasUserReviewedProductAsync(userID, productId);
        }

        public async Task<bool> UpdateReviewAsync(string userID, int reviewId, EditReviewDTO model)
        {
            var result = await _unitOfWork.ReviewRepo.UpdateReviewAsync(userID, reviewId,model);
            if (!result) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
