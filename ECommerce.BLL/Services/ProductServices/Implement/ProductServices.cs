using AutoMapper;
using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.DTOs.HomeDTOs;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.FavoriteService.Interface;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using ECommerce_Tawj.ViewModels.ProductsVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Tawj.Services.ProductServices.Implement
{
    public class ProductServices : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment; // عشان نوصل لمسار الصور (wwwroot)
        private readonly IFavoriteService _favoriteService; 
        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFavoriteService favoriteService)
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

        public async Task<ShopDTO> GetShopProductsAsync(string? searchTerm, int? categoryId, string? userId, int pageNumber = 1, int pageSize = 9)
        {
            var query =  _unitOfWork.ProductRepo.GetAllQueryable(); // IQeryable include category
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || p.Description.ToLower().Contains(searchTerm));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            int totalProducts = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            if (totalPages > 0 && pageNumber > totalPages) pageNumber = totalPages;

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            HashSet<int> favoriteProductIds = new HashSet<int>();
            if (!string.IsNullOrEmpty(userId))
            {
                var favorites = await _unitOfWork.FavoriteRepo.GetFavoritesByUserIdAsync(userId);
                favoriteProductIds = favorites.Select(f => f.ProductId).ToHashSet();
            }

            var productDtos = products.Select(p => new ProductCardDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.Images.FirstOrDefault()?.ImageUrl ?? "/images/default-product.png",
                CategoryName = p.Category != null ? p.Category.Name : "General",
                IsFavorite = favoriteProductIds.Contains(p.Id)
            }).ToList();

            var categories = await _unitOfWork.CategoryRepo.GetAllAsync(null);
            var categoryDtos = categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToList();

            return new ShopDTO
            {
                Products = productDtos,
                Categories = categoryDtos,
                SelectedCategoryId = categoryId,
                SearchTerm = searchTerm ?? string.Empty,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };
        }
        public async Task<ProductEditDTO?> GetProductForEditAsync(int id)
        {
            var product = await _unitOfWork.ProductRepo.GetProductWithDetailsByIdAsync(id);
            if (product == null) return null;

            var categories = await _unitOfWork.CategoryRepo.GetAllAsync(null);

            return new ProductEditDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.Images.FirstOrDefault()?.ImageUrl ?? "/images/default-product.png",
                Categories = _mapper.Map<IEnumerable<CategoryDTO>>(categories)
            };
        }
        public async Task UpdateProductAsync(ProductEditDTO model)
        {
            var product = await _unitOfWork.ProductRepo.GetByIdAsync(model.Id);
            if (product == null) return;

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;

            // حفظ الصورة الجديدة إذا تم رفعها
            if (model.NewImage != null && model.NewImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NewImage.CopyToAsync(stream);
                }

                product.Images.Add(new ProductImage { ImageUrl = "/images/products/" + fileName });
            }

            _unitOfWork.ProductRepo.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepo.GetByIdAsync(id);
            if (product == null) return false;

            await _unitOfWork.ProductRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
