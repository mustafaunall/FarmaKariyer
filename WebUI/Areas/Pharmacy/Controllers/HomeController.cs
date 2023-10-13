using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Pharmacy.Controllers
{
    [Area("Pharmacy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
