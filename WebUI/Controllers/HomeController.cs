using DataAccess.Context;
using Domain.Model;
using Domain.ViewModel;
using Domain.ViewModel.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

//[Authorize]
public class HomeController : Controller
{
    private readonly UserDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public HomeController(UserDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
	{
		var defaultSql = _context.Adverts
			.Include(x => x.ApplicationUser)
            .Where(x => x.IsActive == true)
            .AsQueryable();
		var boostedSql = _context.Adverts
			.Include(x => x.ApplicationUser)
			.Where(x => x.IsActive == true && x.IsBoosted == true)
			.AsQueryable();
		HomeVM vm = new()
        {
            Last3Advert = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Take(3)
            .ToList(),
            BoostedAdvertsTechnician = new(),
            BoostedAdvertsDermocosmetic = new(),
            BoostedAdvertsIntern = new(),
            BoostedAdvertsLicense = new(),
            BoostedAdvertsOther = new(),
            PackagePrice1 = _context.AdvertCategories.Where(x => x.Id == 1).Select(x => x.Price).FirstOrDefault(),
            PackagePrice2 = _context.AdvertCategories.Where(x => x.Id == 2).Select(x => x.Price).FirstOrDefault(),
            PackagePrice3 = _context.AdvertCategories.Where(x => x.Id == 3).Select(x => x.Price).FirstOrDefault(),
        };
		vm.BoostedAdvertsTechnician.AddRange(boostedSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.TECHNICIAN).ToList());
		vm.BoostedAdvertsTechnician.AddRange(defaultSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.TECHNICIAN).ToList());


		vm.BoostedAdvertsDermocosmetic.AddRange(boostedSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.DERMOCOSMETİC).ToList());
		vm.BoostedAdvertsDermocosmetic.AddRange(defaultSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.DERMOCOSMETİC).ToList());


		vm.BoostedAdvertsIntern.AddRange(boostedSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.INTERN).ToList());
		vm.BoostedAdvertsIntern.AddRange(defaultSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.INTERN).ToList());


		vm.BoostedAdvertsLicense.AddRange(boostedSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.LICENSE).ToList());
		vm.BoostedAdvertsLicense.AddRange(defaultSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.LICENSE).ToList());


		vm.BoostedAdvertsOther.AddRange(boostedSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.OTHER).ToList());
		vm.BoostedAdvertsOther.AddRange(defaultSql
			.Where(x => x.Type == Domain.Model.Enum.AdvertType.OTHER).ToList());

        string redirectUrl = "/Account/LoginPharmacy";
        if (User.Identity.IsAuthenticated)
        {
            var u = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
                .SingleAsync(x => x.Email == u.Email);

            if (user != null)
            {
                if (user.Type == ApplicationUserType.PHARMACY)
                {
                    redirectUrl = "/Pharmacy/Account/Packages";
                }
                else if (user.Type == ApplicationUserType.USER)
                {
                    redirectUrl = "/User/Account/Packages";
                }
            }
        }

        ViewBag.RedirectUrl = redirectUrl;

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
    public IActionResult Policies()
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