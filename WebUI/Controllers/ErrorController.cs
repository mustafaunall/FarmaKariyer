using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ErrorController : Controller
{
    public IActionResult ErrorHandler(int statusCode)
    {
        if (statusCode == 404)
            return View("PageNotFound");
        if (statusCode == 500)
            return View("PageBadRequest");
        return View();
    }
}
