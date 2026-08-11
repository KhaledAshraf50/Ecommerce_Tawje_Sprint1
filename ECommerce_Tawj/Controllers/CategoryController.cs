using ECommerce_Tawj.DTOs.CategoryDTOs;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields correctly.";
                return RedirectToAction(nameof(Index));
            }

            await _categoryService.UpdateCategoryAsync(model);
            TempData["Success"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result)
            {
                TempData["Success"] = "Category deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete category.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
