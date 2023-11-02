using DataAccess.Context;
using Domain.Model;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Extensions;
using WebUI.Models.ViewModels;

namespace WebUI.Areas.Pharmacy.Controllers
{
    [Area("Pharmacy")]
    public class AdvertController : BaseController
    {
        private readonly UserDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdvertController(
            UserDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> List()
        {
            var u = await _userManager.GetUserAsync(User);
            var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

            var model = await _context.Adverts
                .Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == user!.Id && x.IsActive)
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var u = await _userManager.GetUserAsync(User);
            var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();
            if (user == null)
            {
                Notification("Kullanıcı bulunamadı!", NotificationType.Error);
            }

            var userAdvertQuota = user!.AdvertPostingQuota;
            if (userAdvertQuota < 1)
            {
                Notification("İlan verme hakkınız yok, lütfen paket alımı gerçekleştiriniz.", NotificationType.Info);
                return Redirect("/Pharmacy/Account/Packages");
            }
            return View();
        }

        public async Task<IActionResult> ApplyList()
        {
            var u = await _userManager.GetUserAsync(User);
            var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();
            if (user == null)
            {
                Notification("Kullanıcı bulunamadı!", NotificationType.Error);
            }

            var model = await _context.Applies
                    .Include(x => x.ApplicantUser)
                    .Include(x => x.CurrentResume)
                    .Include(x => x.Advert)
                    .Where(x => x.Advert.ApplicationUser.Id == user!.Id)
                    .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Passive([FromForm] int id)
        {
            var advert = await _context.Adverts.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (advert == null)
            {
                Notification("İlan bulunamadı!", NotificationType.Error);
            }
            else
            {
                advert.IsActive = false;
                await _context.SaveChangesAsync();
                Notification("İlan başarıyla silindi", NotificationType.Success);
            }

            return RedirectPermanent("/Pharmacy/Advert/List");
        }

        public IActionResult ApplyDetail(int id)
        {
            var model = _context.Users
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertCreateVM model)
        {
            try
            {
                var u = await _userManager.GetUserAsync(User);
                var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

                var userAdvertQuota = user!.AdvertPostingQuota;
                if (userAdvertQuota < 1)
                {
                    Notification("İlan verme hakkınız yok, lütfen paket alımı gerçekleştiriniz.", NotificationType.Info);
                    return Redirect("/Pharmacy/Account/Packages");
                }

                Advert advert = new();

                advert.ApplicationUserId = user!.Id;
                advert.Title = model.Title;
                advert.Description = model.Description;
                advert.Type = (AdvertType)model.Type;
                advert.IsBoosted = model.IsBoosted;
                if (model.Type == AdvertType.TECHNICIAN)
                {
                    advert.ExperienceYear = model.ExperienceYear;
                    advert.BonusBenefit = model.BonusBenefit;
                    advert.TravelBenefit = model.TravelBenefit;
                    advert.FoodBenefit = model.FoodBenefit;
                    advert.SalaryRange = model.SalaryRange;
                    advert.PrescriptionInfo = model.PrescriptionInfo;
                    advert.PrivateInsuranceEntryInfo = model.PrivateInsuranceEntryInfo;
                    advert.OTCInfo = model.OTCInfo;
                    advert.DermocosmeticInfo = model.DermocosmeticInfo;
                    if (model.DermocosmeticInfo == true)
                        advert.IsDermocosmetic = true;
                    advert.OtherInfo = model.OtherInfo;
                }
                else if (model.Type == AdvertType.INTERN)
                {
                    advert.EducationStatus = model.EducationStatus;
                }
                else if (model.Type == AdvertType.ASSISTANT)
                {
                    advert.EducationStatus = model.EducationStatus;
                }
                else if (model.Type == AdvertType.LICENSE)
                {
                    advert.SquareMeter = model.SquareMeter;
                    advert.MonthlyTurnover = model.MonthlyTurnover;
                    advert.WithLicenseRight = model.WithLicenseRight;
                    advert.WithoutLicenseRight = model.WithoutLicenseRight;
                    advert.HasRightToCarry = model.HasRightToCarry;
                }
                else if (model.Type == AdvertType.OTHER)
                {
                    advert.DriverLicenses = model.DriverLicenses;
                }

                await _context.AddAsync(advert);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    //user.ResumeId = advert.Id;
                    user.AdvertPostingQuota = user.AdvertPostingQuota - 1;
                    await _context.SaveChangesAsync();
                    Notification("İlan başarıyla yayınlandı.", NotificationType.Success);
                    return Redirect("/Pharmacy/Advert/List");
                }
            }
            catch (Exception ex)
            {
                Notification("İlan yayınlanırken bir hata meydana geldi!", NotificationType.Error);
                return View(model);
            }
            return View(model);
        }
    }
}
