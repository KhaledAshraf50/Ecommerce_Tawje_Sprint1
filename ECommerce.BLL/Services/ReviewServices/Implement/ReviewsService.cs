using AutoMapper;
using ECommerce_Tawj.DTOs.ReviewDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.ReviewServices.Interfaces;

namespace ECommerce_Tawj.Services.ReviewServices.Implement
{
    public class ReviewsService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _unitOfWork.ReviewRepo.GetProductReviewsAsync(productId);
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<bool> AddReviewAsync(string userID, AddReviewDTO model)
        {
            var reviewModel = _mapper.Map<Review>(model);

            var result = await _unitOfWork.ReviewRepo.AddReviewAsync(userID, reviewModel);
            if (!result) return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReviewAsync(string userID, int reviewId, EditReviewDTO model)
        {
            var reviewModel = _mapper.Map<Review>(model);

            var result = await _unitOfWork.ReviewRepo.UpdateReviewAsync(userID, reviewId, reviewModel);
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
    }
}