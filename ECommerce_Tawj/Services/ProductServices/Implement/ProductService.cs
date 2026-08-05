using AutoMapper;
using ECommerce_Tawj.DTOs.HomeDTOs;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.FavoriteService.Interface;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using ECommerce_Tawj.ViewModels.ProductsVM;

namespace ECommerce_Tawj.Services.ProductServices.Implement
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment; // عشان نوصل لمسار الصور (wwwroot)
        private readonly IFavoriteService _favoriteService; 
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFavoriteService favoriteService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _favoriteService = favoriteService;
        }
        public Task<IEnumerable<Product>> GetProductWithCategoriesWithProImagesAsync()
        {
            return _unitOfWork.ProductRepo.GetProductWithCategoriesWithProImages();
        }
        public async Task AddProductAsync(CreateProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            if(productDto.Images != null && productDto.Images.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder); // if folder not exist create it
                bool isFirstImage = true;

                foreach(var file in productDto.Images)
                {
                    if (file.Length>0)
                    {
                        // create uniqe name for each file 
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        // create the full path for the file
                        string filePath = Path.Combine(uploadsFolder,uniqueFileName);
                        // open connect with the file and save it in the path
                        using(var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //product.Images.Add(new ProductImage
                        //{
                        //    ImageUrl = "/images/products/" + uniqueFileName,
                        //    IsMain = isFirstImage
                        //});
                        _unitOfWork.ProductImageRepo.Add(new ProductImage
                        {
                            ImageUrl = "/images/products/" + uniqueFileName,
                            IsMain = isFirstImage,
                            Product = product
                        });
                        isFirstImage = false; // بعد ما نحفظ الصورة الاولى نخلي الباقي false
                    }
                }
            }
            _unitOfWork.ProductRepo.Add(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<HomeDTO> GetHomePageDataAsync(string? userId)
        {
            var products = await _unitOfWork.ProductRepo.GetProductWithCategoriesWithProImages();

            var favoriteProductIds = !string.IsNullOrEmpty(userId)
                ? await _favoriteService.GetUserFavoriteProductIdsAsync(userId)
                : new List<int>();

            var allProductDtos = _mapper.Map<List<ProductsHomeDTO>>(products);

            // تعليم المنتجات المفضلة
            foreach (var dto in allProductDtos)
            {
                dto.IsFavorite = favoriteProductIds.Contains(dto.Id);
            }

            return new HomeDTO
            {
                HeroDeals = allProductDtos
                .Where(p => p.DiscountPercentage > 0)
                .OrderByDescending(p => p.DiscountPercentage)
                .Take(3),
                PopularProducts = allProductDtos
                .OrderByDescending(p => p.AverageRating)
                .Take(8),
                FeaturedProducts = allProductDtos
                .Where(p => p.AverageRating >= 4.0)
                .Take(3)
            };
        }

        public async Task<ProductDetailsDTO?> GetProductDetailsByIdAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepo.GetProductWithDetailsByIdAsync(productId);
            if (product == null) return null;
            return _mapper.Map<ProductDetailsDTO>(product);
        }
    }
}
