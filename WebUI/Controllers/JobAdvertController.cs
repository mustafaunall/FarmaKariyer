using DataAccess.Context;
using Domain.Model.Enum;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class JobAdvertController : Controller
{
    private readonly UserDbContext _context;
    public JobAdvertController(UserDbContext context)
    {
        _context = context;
    }

    public IActionResult Pharmacy()
    {
        return View();
    }
    public IActionResult Technician()
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Type == AdvertType.TECHNICIAN)
            .ToList();

        return View(model);
    }
    public IActionResult Intern()
    {
        return View();
    }
    public IActionResult License()
    {
        return View();
    }
    public IActionResult Detail(int id)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Type == AdvertType.TECHNICIAN && x.Id == id)
            .FirstOrDefault();
        return View(model);
    }
    public IActionResult OtherAds()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}