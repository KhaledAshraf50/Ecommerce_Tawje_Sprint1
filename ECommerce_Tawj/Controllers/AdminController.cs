using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
