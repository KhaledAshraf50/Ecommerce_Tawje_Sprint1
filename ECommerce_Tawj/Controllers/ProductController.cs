using AutoMapper;
using ECommerce_Tawj.DTOs.ProductsDTOs;
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using ECommerce_Tawj.ViewModels.ProductsVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductWithCategoriesWithProImagesAsync();
            List<ProductDTO> productsDto = _mapper.Map<List<ProductDTO>>(products);
            return View(productsDto);
        }
        [HttpGet]
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
            return RedirectToAction("Index");
        }
        public IActionResult AllProduct()
        {
            return View(); 
        }
        public async Task<IActionResult> ProductDetails(int productId)
        {
            if (productId <= 0)
            {
                return NotFound();
            }

            var productDto = await _productService.GetProductDetailsByIdAsync(productId);
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
