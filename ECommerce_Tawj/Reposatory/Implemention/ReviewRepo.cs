using AutoMapper;
using ECommerce_Tawj.DTOs.ReviewDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class ReviewRepo : GenricRepo<Review>, IReviewRepo
    {
        private readonly IMapper _mapper;
        public ReviewRepo(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _context.Review
                            .Where(r=>r.ProductId == productId)
                            .Include(r=>r.User)
                            .OrderByDescending(r=>r.CreatedAt)
                            .AsNoTracking()
                            .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        
        }
        public async Task<bool> HasUserReviewedProductAsync(string userID, int productId)
        {
            return await _context.Review
                .AsQueryable()
                .AnyAsync(r => r.UserId == userID && r.ProductId == productId);
        }

        public async Task<bool> AddReviewAsync(string userID, AddReviewDTO model)
        {
            // prevent duplicate review
            var existingReview = await GetUserReviewForProductAsync(userID, model.ProductId);
            if (existingReview != null)
            {
                if (existingReview.IsDeleted)
                {
                    existingReview.IsDeleted = false;
                    existingReview.Comment = model.Comment;
                    existingReview.Rating = model.Rating;
                    return true;
                }
                return false;
            }
               

            var review = _mapper.Map<Review>(model);

            review.UserId = userID;

            Add(review);
            return true;
        }
        public async Task<bool> UpdateReviewAsync(string userID, int reviewId, EditReviewDTO model)
        {
            var review = await _context.Review
                           .FirstOrDefaultAsync(r =>
                               r.Id == reviewId &&
                               r.UserId == userID);
            if (review == null)
                return false;

            review.Comment = model.Comment;
            review.Rating = model.Rating;

            Update(review);

            return true;
        }

        public async Task<bool> DeleteReviewAsync(string userID, int reviewId)
        {
            var review = await _context.Review
                .FirstOrDefaultAsync(r =>
                    r.Id == reviewId &&
                    r.UserId == userID);

            if (review == null)
                return false;

            await DeleteAsync(reviewId);

            return true;
        }

        public async Task<Review?> GetUserReviewForProductAsync(string userId, int productId)
        {
            return await _context.Review
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r=>r.UserId == userId &&  r.ProductId == productId);
        }
    }
}
