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

    public IActionResult Detail(int id)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Id == id)
            .FirstOrDefault();
        return View(model);
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
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Type == AdvertType.INTERN)
            .ToList();

        return View(model);
    }
    public IActionResult License()
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Type == AdvertType.LICENSE)
            .ToList();

        return View(model);
    }

    public IActionResult OtherAds()
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Type == AdvertType.OTHER)
            .ToList();

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}