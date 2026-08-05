using AutoMapper;
using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;

namespace ECommerce_Tawj.Services.CategoryServices.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddCategoryAsync(AddCategoryDTO model)
        {
            var category = _mapper.Map<Category>(model);
            _unitOfWork.CategoryRepo.Add(category);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepo.GetCategoriesWithProductsAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
    }
}
