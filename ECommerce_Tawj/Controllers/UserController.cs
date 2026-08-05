using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
