
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class ReviewRepo : GenricRepo<Review>, IReviewRepo
    {
        public ReviewRepo(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _context.Review
                            .Where(r=>r.ProductId == productId)
                            .Include(r=>r.User)
                            .OrderByDescending(r=>r.CreatedAt)
                            .AsNoTracking()
                            .ToListAsync();
        
        }
        public async Task<bool> HasUserReviewedProductAsync(string userID, int productId)
        {
            return await _context.Review
                .AsQueryable()
                .AnyAsync(r => r.UserId == userID && r.ProductId == productId);
        }

        public async Task<bool> AddReviewAsync(string userID, Review model)
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

            model.UserId = userID;

            Add(model);
            return true;
        }
        public async Task<bool> UpdateReviewAsync(string userID, int reviewId, Review model)
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
