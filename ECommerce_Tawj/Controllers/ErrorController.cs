using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Tawj.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("~/Views/Shared/NotFound.cshtml");
        }
        [Route("Error/403")]
        public IActionResult AccessDeniedPage()
        {
            return View("~/Views/Account/AccessDenied.cshtml");
        }
    }
}
