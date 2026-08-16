using AutoMapper;
using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce_Tawj.Services.CategoryServices.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private const string CategoriesCacheKey = "categories";

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper,IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task AddCategoryAsync(AddCategoryDTO model)
        {
            var category = _mapper.Map<Category>(model);
            _unitOfWork.CategoryRepo.Add(category);
            await _unitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CategoriesCacheKey);
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            if(_memoryCache.TryGetValue(CategoriesCacheKey,out IEnumerable<CategoryDTO>? categories))
            {
                return categories!;
            }
            var categoriesFromDb = await _unitOfWork.CategoryRepo.GetCategoriesWithProductsAsync();
            var result = _mapper.Map<IEnumerable<CategoryDTO>>(categoriesFromDb);
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            _memoryCache.Set(CategoriesCacheKey, result, options);
            return result;
        }
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDTO>(category);
        }
        public async Task UpdateCategoryAsync(CategoryDTO categoryDto)
        {
            var category = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryDto.Id);
            if (category != null)
            {
                _mapper.Map(categoryDto, category);
                _unitOfWork.CategoryRepo.Update(category);
                await _unitOfWork.SaveChangesAsync();
                _memoryCache.Remove(CategoriesCacheKey);
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            await _unitOfWork.CategoryRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CategoriesCacheKey);
            return true;
        }
    }
}
