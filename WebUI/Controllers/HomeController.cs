using DataAccess.Context;
using Domain.ViewModel;
using Domain.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class HomeController : Controller
{
    private readonly UserDbContext _context;
    public HomeController(UserDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var boostedSql = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsBoosted == true)
            .AsQueryable();
        HomeVM vm = new()
        {
            Last3Advert = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Take(3)
            .ToList(),
            BoostedAdvertsTechnician = boostedSql
            .Where(x => x.Type == Domain.Model.Enum.AdvertType.TECHNICIAN).ToList(),
            BoostedAdvertsDermocosmetic = boostedSql
            .Where(x => x.Type == Domain.Model.Enum.AdvertType.TECHNICIAN).ToList(),
            BoostedAdvertsIntern = boostedSql
            .Where(x => x.Type == Domain.Model.Enum.AdvertType.INTERN).ToList(),
            BoostedAdvertsLicense = boostedSql
            .Where(x => x.Type == Domain.Model.Enum.AdvertType.LICENSE).ToList(),
            BoostedAdvertsOther = boostedSql
            .Where(x => x.Type == Domain.Model.Enum.AdvertType.OTHER).ToList(),
        };

        return View(vm);
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