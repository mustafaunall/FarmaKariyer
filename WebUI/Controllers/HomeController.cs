using DataAccess.Context;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class HomeController : Controller
{
    private readonly UserDbContext _userContext;
    public HomeController(UserDbContext userContext)
    {
        _userContext = userContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Faq()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult OurPartners()
    {
        return View();
    }
    public IActionResult AboutUs()
    {
        return View();
    }
    public IActionResult Pricing()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}