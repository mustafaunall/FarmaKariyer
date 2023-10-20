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

            var applyCheck = _context.Applies.Where(x => x.ApplicantUserId == user.Id).FirstOrDefault();
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
            .Where(x => x.Id == id)
            .FirstOrDefault();
        ViewBag.AdvertId = model?.Id;
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