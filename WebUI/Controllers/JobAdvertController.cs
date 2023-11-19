using DataAccess.Context;
using Domain.Model;
using Domain.Model.Enum;
using Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebUI.Extensions;

namespace WebUI.Controllers;

//[Authorize]
public class JobAdvertController : BaseController
{
    private readonly UserDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private const int _pageLimit = 6;
    public JobAdvertController(
        UserDbContext context,
        UserManager<ApplicationUser> userManager
        )
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Apply([FromForm] int AdvertId)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                Notification("Başvuru özelliğini kullanmak için lütfen giriş yapınız", NotificationType.Info);
                return Redirect($"/Account/LoginUser");
            }

            var advert = await _context.Adverts.FirstOrDefaultAsync(x => x.Id == AdvertId);

            var u = await _userManager.GetUserAsync(User);
            var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

            if (user == null)
            {
                Notification("Kullanıcı bulunamadı, lütfen giriş yapın!", NotificationType.Error);
                return Redirect($"/Account/LoginUser");
            }

            if (user.Type == ApplicationUserType.PHARMACY)
            {
                Notification("Başvuru özelliği eczaneler için geçerli değil, sadece kullanıcılar içindir", NotificationType.Info);
                return Redirect($"/JobAdvert/Detail/{AdvertId}");
            }

            var applyCheck = _context.Applies.Where(x => x.ApplicantUserId == user.Id && x.AdvertId == AdvertId).FirstOrDefault();
            if (applyCheck != null)
            {
                // TODO: Bir ilana birden fazla başvuru olacak mı ?
                Notification("Bir ilana sadece bir kere başvuruda bulunabilirsiniz", NotificationType.Info);
                return Redirect($"/JobAdvert/Detail/{AdvertId}");
            }

            var resumeCheck = _context.Users.Where(x => x.Id == user.Id && x.ActiveResumeId != null).FirstOrDefault();
            if (resumeCheck == null)
            {
                Notification("İlana başvurmak için özgeçmişinizi sisteme ekleyiniz", NotificationType.Info);
                return Redirect($"/JobAdvert/Detail/{AdvertId}");
            }

            Apply apply = new()
            {
                AdvertId = advert.Id,
                ApplicantUserId = user.Id,
                ApplyDate = DateTime.Now,
                CurrentResumeId = (int)user.ActiveResumeId!,
            };
            await _context.AddAsync(apply);
            await _context.SaveChangesAsync();

            Notification("İlana başarıyla başvuruldu", NotificationType.Success);
            return Redirect($"/JobAdvert/Detail/{AdvertId}");
        }
        catch (Exception ex)
        {
            Notification("İlana başvururken bir hata meydana geldi!", NotificationType.Error);
            return Redirect($"/JobAdvert/Detail/{AdvertId}");
        }
    }

    public IActionResult Detail(int id)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .Where(x => x.Id == id && x.IsActive)
            .FirstOrDefault();
        if (model == null)
        {
            return Redirect("/Error/ErrorHandler?statusCode=404");
        }
        ViewBag.AdvertId = model?.Id;
        return View(model);
    }

    public IActionResult Technician([FromQuery] int page = 1)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Skip((page - 1) * _pageLimit)
            .Take(_pageLimit)
            .Where(x => x.Type == AdvertType.TECHNICIAN && x.IsActive)
            .ToList();
        var modelCount = _context.Adverts
            .Where(x => x.Type == AdvertType.TECHNICIAN && x.IsActive)
            .Count();
        ViewBag.Page = page;
        ViewBag.PageCount = modelCount % _pageLimit == 0 ? (modelCount / _pageLimit) : (modelCount / _pageLimit + 1);
        return View(model);
    }

    public IActionResult Dermocosmetic([FromQuery] int page = 1)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Skip((page - 1) * _pageLimit)
            .Take(_pageLimit)
            .Where(x => x.Type == AdvertType.DERMOCOSMETİC && x.IsActive)
            .ToList();
        var modelCount = _context.Adverts
            .Where(x => x.Type == AdvertType.DERMOCOSMETİC && x.IsActive)
            .Count();
        ViewBag.Page = page;
        ViewBag.PageCount = modelCount % _pageLimit == 0 ? (modelCount / _pageLimit) : (modelCount / _pageLimit + 1);
        return View(model);
    }

    public IActionResult Intern([FromQuery] int page = 1)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Skip((page - 1) * _pageLimit)
            .Take(_pageLimit)
            .Where(x => x.Type == AdvertType.INTERN && x.IsActive)
            .ToList();
        var modelCount = _context.Adverts
            .Where(x => x.Type == AdvertType.INTERN && x.IsActive)
            .Count();
        ViewBag.Page = page;
        ViewBag.PageCount = modelCount % _pageLimit == 0 ? (modelCount / _pageLimit) : (modelCount / _pageLimit + 1);
        return View(model);
    }
    public IActionResult License([FromQuery] int page = 1)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Skip((page - 1) * _pageLimit)
            .Take(_pageLimit)
            .Where(x => x.Type == AdvertType.LICENSE && x.IsActive)
            .ToList();
        var modelCount = _context.Adverts
            .Where(x => x.Type == AdvertType.LICENSE && x.IsActive)
            .Count();
        ViewBag.Page = page;
        ViewBag.PageCount = modelCount % _pageLimit == 0 ? (modelCount / _pageLimit) : (modelCount / _pageLimit + 1);
        return View(model);
    }

    public IActionResult OtherAds([FromQuery] int page = 1)
    {
        var model = _context.Adverts
            .Include(x => x.ApplicationUser)
            .OrderByDescending(x => x.CreateDate)
            .Skip((page - 1) * _pageLimit)
            .Take(_pageLimit)
            .Where(x => x.Type == AdvertType.OTHER && x.IsActive)
            .ToList();
        var modelCount = _context.Adverts
            .Where(x => x.Type == AdvertType.OTHER && x.IsActive)
            .Count();
        ViewBag.Page = page;
        ViewBag.PageCount = modelCount % _pageLimit == 0 ? (modelCount / _pageLimit) : (modelCount / _pageLimit + 1);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}