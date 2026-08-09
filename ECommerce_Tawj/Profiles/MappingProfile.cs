using AutoMapper;
using ECommerce_Tawj.DTOs.AccountDTOs;
using ECommerce_Tawj.DTOs.CartItemDTOs;
using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.DTOs.OrdersDTOs;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.ViewModels.ProductsVM;

namespace ECommerce_Tawj.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(img => img.IsMain) != null
                ? src.Images.FirstOrDefault(img => img.IsMain)!.ImageUrl
                : "/images/no-image.jpg"));
            CreateMap<ProductDTO, Product>();
            CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // تجاهل التحويل التلقائي للصور
            // Category Mappings
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.Products.Count));
            CreateMap<AddCategoryDTO, Category>();
            // Register Mappings
            CreateMap<RegisterUserDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.fullName));
            // HomeProducts Mappings
            CreateMap<Product, ProductsHomeDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(img => img.IsMain) != null
                ? src.Images.FirstOrDefault(img => img.IsMain)!.ImageUrl
                : "/images/no-image.jpg"));
            // productImage
            CreateMap<ProductImage, ProductImageDTO>();
            // productDetail
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.ProductName, ops => ops.MapFrom(src => src.Name))
                .ForMember(dest=>dest.ProductDescription,ops=>ops.MapFrom(src=>src.Description))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
            // Cart Mapping
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src =>
                    src.Product.DiscountPercentage > 0 && src.Product.DiscountPercentage <= 100
                    ? src.Product.Price - (src.Product.Price * src.Product.DiscountPercentage / 100m)
                    : src.Product.Price))
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                    src.Product.Images.FirstOrDefault(img => img.IsMain) != null
                    ? src.Product.Images.FirstOrDefault(img => img.IsMain)!.ImageUrl
                    : "/images/no-image.jpg"));

            // 1. تحويل عناصر السلة إلى عناصر الطلب
            CreateMap<CartItemDTO, OrderItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.ProductPrice));

            // 2. تحويل CheckoutDTO إلى Order (يشمل تحويل القائمة تلقائياً)
            CreateMap<CheckoutDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Cart.Items));

            // 3. تحويل Order إلى DTO الخاص بالأدمن
            CreateMap<Order, AdminOrderDTO>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.ShippingFirstName} {src.ShippingLastName}"))
            .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.OrderItems.Count))
            .ForMember(dest => dest.ItemsSummary, opt => opt.MapFrom(src =>
                 string.Join(", ", src.OrderItems.Select(i => $"{i.Product.Name} x {i.Quantity}"))));

            // Mapping for Recent Orders in Admin Dashboard
            CreateMap<Order, RecentOrderDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.ShippingFirstName} {src.ShippingLastName}"));
        }
    }
}
