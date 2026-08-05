using AutoMapper;
using ECommerce_Tawj.DTOs.AccountDTOs;
using ECommerce_Tawj.DTOs.CategoryDTOs;
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
            // 
            CreateMap<ProductImage, ProductImageDTO>();
            // 
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.ProductName, ops => ops.MapFrom(src => src.Name))
                .ForMember(dest=>dest.ProductDescription,ops=>ops.MapFrom(src=>src.Description))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
        }
    }
}
