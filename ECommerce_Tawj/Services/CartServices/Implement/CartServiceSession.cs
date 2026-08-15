using AutoMapper;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CartServices.Interfaces;
using System.Text.Json;

namespace ECommerce_Tawj.Services.CartServices.Implement
{
    public class CartServiceSession : ICartServiceSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private const string CartKey = "Cart";

        public CartServiceSession
            (IHttpContextAccessor httpContextAccessor
            ,IUnitOfWork unitOfWork
            ,IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // Helper Methods Prevent Code Redundancy
        private List<SessionCartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext!.Session;

            var json = session.GetString(CartKey);

            if (string.IsNullOrEmpty(json))
                return new List<SessionCartItem>();

            return JsonSerializer.Deserialize<List<SessionCartItem>>(json)
            ?? new List<SessionCartItem>();
        }
        private void SaveCart(List<SessionCartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext!.Session;

            var json = JsonSerializer.Serialize(cart);

            session.SetString(CartKey, json);
        }
        public async Task AddItemAsync(int productId, int quantity = 1)
        {
            var product = await _unitOfWork.ProductRepo.GetProductWithDetailsByIdAsync(productId);
            if(product == null)  throw new Exception("Product Not Found!");

            var Cart = GetCartItems();

            var existingCart = Cart.FirstOrDefault(x=>x.ProductId == productId);

            if (existingCart != null)
            {
                existingCart.Quantity = quantity;
            }
            else
            {
                var sessionCartItem = _mapper.Map<SessionCartItem>(product);
                sessionCartItem.Quantity = quantity;

                Cart.Add(sessionCartItem);
            }
            SaveCart(Cart);
        }
        public List<SessionCartItem> GetCart()
        {
            return GetCartItems();
        }
        public  Task IncreaseQuantityAsync(int productId)
        {
            var carts = GetCartItems();
            var item = carts.FirstOrDefault(x=>x.ProductId == productId);
            if(item != null)
            {
                item.Quantity++;
                SaveCart(carts);
            }
            return Task.CompletedTask;
        }
        public  Task DecreaseQuantityAsync(int productId)
        {
            var carts = GetCartItems();
            var item = carts.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity--;
                if(item.Quantity <= 0)
                {
                    carts.Remove(item);
                }
                SaveCart(carts);
            }
            return Task.CompletedTask;
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(CartKey);
        }

        public int GetCartCount()
        {
            var carts = GetCartItems();
            return carts.Sum(x=>x.Quantity);
        }

        public void RemoveItem(int productId)
        {
            var carts = GetCartItems();

            var item = carts.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                carts.Remove(item);
                SaveCart(carts);
            }
        }
    }
}
