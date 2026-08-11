using AutoMapper;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using ECommerce_Tawj.ViewModels.ProductsVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ECommerce_Tawj.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> AllProduct(string? searchTerm, int? categoryId, int pageNumber = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var model = await _productService.GetShopProductsAsync(searchTerm, categoryId, userId, pageNumber);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductWithCategoriesWithProImagesAsync();
            List<ProductDTO> productsDto = _mapper.Map<List<ProductDTO>>(products);
            return View(productsDto);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategories();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO model)
        {
            if (!ModelState.IsValid)
            {
               ViewBag.Categories = await GetCategories();
                return View(model);
            }
            await _productService.AddProductAsync(model);
            TempData["Success"] = $"{model.Name} Added Successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productService.GetProductForEditAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditDTO model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                model.Categories = categories;
                return View(model);
            }

            await _productService.UpdateProductAsync(model);
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result)
            {
                TempData["Success"] = "Product deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete product.";
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductDetails(int Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }

            var productDto = await _productService.GetProductDetailsByIdAsync(Id);
            if (productDto == null)
            {
                return NotFound();
            }
            return View(productDto);
        }
        public async Task<SelectList> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return new SelectList(categories, "Id", "Name");
        }
    }
}
