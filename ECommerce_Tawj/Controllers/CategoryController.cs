using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService; 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new CategoryIndexDTO
            {
                Categories = await _categoryService.GetAllCategoriesAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryIndexDTO model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetAllCategoriesAsync();
                return View("Index", model);
            }
            await _categoryService.AddCategoryAsync(model.NewCategory);
            return RedirectToAction("Index");
        }
    }
}
