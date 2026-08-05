using AutoMapper;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.FavoriteService.Interface;
using ECommerce_Tawj.ViewModels.ProductsVM;

namespace ECommerce_Tawj.Services.FavoriteService.Implement
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> ToggleFavoriteAsync(string userId, int productId)
        {
            var existingFavorite = await _unitOfWork.FavoriteRepo
                 .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (existingFavorite != null)
            {
                await _unitOfWork.FavoriteRepo.DeleteAsync(existingFavorite.Id);
                await _unitOfWork.SaveChangesAsync();
                return false;
            }
            else
            {
                var newFavorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                };
                 _unitOfWork.FavoriteRepo.Add(newFavorite);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<int>> GetUserFavoriteProductIdsAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId)) return new List<int>();
            var userFavorites = await _unitOfWork.FavoriteRepo
                .GetAllAsync(f => f.UserId == userId);
            return userFavorites.Select(f => f.ProductId).ToList();
        }

        public async Task<IEnumerable<ProductsHomeDTO>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _unitOfWork.FavoriteRepo.GetFavoritesWithProductsAsync(userId);
            var products = favorites.Select(f => f.Product);

            var dtos = _mapper.Map<IEnumerable<ProductsHomeDTO>>(products);
            foreach (var item in dtos)
            {
                item.IsFavorite = true;
            }
            return dtos;
        }

    }
}
