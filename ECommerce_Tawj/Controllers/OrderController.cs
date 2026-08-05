using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
