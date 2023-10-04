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
    private readonly AdminDbContext _adminContext;
    public JobAdvertController(UserDbContext userContext, AdminDbContext adminContext)
    {
        _userContext = userContext;
        _adminContext = adminContext;
    }

    public IActionResult List()
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