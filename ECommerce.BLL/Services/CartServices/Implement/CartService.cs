using AutoMapper;
using ECommerce_Tawj.DTOs.CartItemDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CartServices.Interfaces;

namespace ECommerce_Tawj.Services.CartServices.Implement
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }
        public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
        {
            var existingItem = await _unitOfWork.CartRepo
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _unitOfWork.CartRepo.Update(existingItem);

            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _unitOfWork.CartRepo.Add(newItem);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var items = await _unitOfWork.CartRepo.GetCartItemsByUserIdAsync(userId);
            var itemsDTO = _mapper.Map<IEnumerable<CartItemDTO>>(items);
            return new CartDTO { Items = itemsDTO };
        }
        public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var item = await _unitOfWork.CartRepo.GetByIdAsync(cartItemId);
            if (item != null && item.UserId == userId)
            {
                if(quantity <= 0)
                {
                    await _unitOfWork.CartRepo.DeleteAsync(cartItemId);
                }
                else
                {
                    item.Quantity = quantity;
                    _unitOfWork.CartRepo.Update(item);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var item = await _unitOfWork.CartRepo.GetByIdAsync(cartItemId);
            if (item != null && item.UserId == userId)
            {
                await _unitOfWork.CartRepo.DeleteAsync(cartItemId);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return 0;

            var items = await _unitOfWork.CartRepo.GetCartItemsByUserIdAsync(userId);
            return items.Sum(i => i.Quantity); // حساب مجموع كميات المنتجات
        }

        public async Task RemoveProductFromCartAsync(string userId, int productId)
        {
            var existingItem = await _unitOfWork.CartRepo
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingItem != null)
            {
                await _unitOfWork.CartRepo.DeleteAsync(existingItem.Id);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task ClearCartAsync(string userId)
        {
            var items = await _unitOfWork.CartRepo.GetCartItemsByUserIdAsync(userId);

            foreach (var item in items)
            {
                await _unitOfWork.CartRepo.DeleteAsync(item.Id);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
