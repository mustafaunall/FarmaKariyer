using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult InternList()
        {
            return View();
        }
    }
}
