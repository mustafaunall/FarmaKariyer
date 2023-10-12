using DataAccess.Context;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class JobAdvertController : Controller
{
    private readonly UserDbContext _userContext;
    public JobAdvertController(UserDbContext userContext)
    {
        _userContext = userContext;
    }

    public IActionResult Pharmacy()
    {
        return View();
    }
    public IActionResult Technician()
    {
        return View();
    }
    public IActionResult Intern()
    {
        return View();
    }
    public IActionResult License()
    {
        return View();
    }
    public IActionResult Detail()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}