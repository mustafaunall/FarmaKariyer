using DataAccess.Context;
using DataAccess.Model;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdminUI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserDbContext _context;
    private readonly UserDbContext _userContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ILogger<HomeController> logger,
        UserDbContext context,
        UserDbContext userContext,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userContext = userContext;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}