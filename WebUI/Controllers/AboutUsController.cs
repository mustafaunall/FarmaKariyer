using DataAccess.Context;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class AboutUsController : Controller
{
    public IActionResult Faq()
    {
        return View();
    }

}