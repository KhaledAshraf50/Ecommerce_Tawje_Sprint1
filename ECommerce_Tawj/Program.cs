
using ECommerce_Tawj.Models;
using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Profiles;
using ECommerce_Tawj.Reposatory.Implemention;
using ECommerce_Tawj.Reposatory.Interfaces;
using ECommerce_Tawj.Services.AccountServices.Implement;
using ECommerce_Tawj.Services.AccountServices.Interfaces;
using ECommerce_Tawj.Services.AdminServices.Implement;
using ECommerce_Tawj.Services.AdminServices.Interfaces;
using ECommerce_Tawj.Services.CartServices.Implement;
using ECommerce_Tawj.Services.CartServices.Interfaces;
using ECommerce_Tawj.Services.CategoryServices.Implement;
using ECommerce_Tawj.Services.CategoryServices.Interfaces;
using ECommerce_Tawj.Services.EmailService;
using ECommerce_Tawj.Services.FavoriteService.Implement;
using ECommerce_Tawj.Services.FavoriteService.Interface;
using ECommerce_Tawj.Services.FilesService;
using ECommerce_Tawj.Services.OrderServices.Implement;
using ECommerce_Tawj.Services.OrderServices.Interfaces;
using ECommerce_Tawj.Services.ProductServices.Implement;
using ECommerce_Tawj.Services.ProductServices.Interfaces;
using ECommerce_Tawj.Services.ReviewServices.Implement;
using ECommerce_Tawj.Services.ReviewServices.Interfaces;
using ECommerce_Tawj.Services.UserServices.Implement;
using ECommerce_Tawj.Services.UserServices.Interfaces;
using ECommerceBLL.DataSeeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the ApplicationDbContext with SQL Server provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Error/403";
});
// add Stripe Service
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
// regester the UnitOfWork 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Add AutoMapper
builder.Services.AddAutoMapper(cfg=> { },typeof(MappingProfile));
// add services 
builder.Services.AddScoped<IProductService, ProductServices>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, AccountServices>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, LocalFileService>();
builder.Services.AddScoped<ICartServiceSession, CartServiceSession>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReviewService, ReviewsService>();

// Add Session Settings For Task2 Sprint2

builder.Services.AddDistributedMemoryCache(); // reserve A Space In Memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddMemoryCache();

// add SmtpSettings

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// Data Seeding

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding data : {ex.Message}");
    }
}

app.Run();
