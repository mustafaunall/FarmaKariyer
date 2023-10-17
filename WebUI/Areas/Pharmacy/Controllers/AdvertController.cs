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

            var model = await _context.Adverts.Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == user!.Id).ToListAsync();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Apply()
        {
            var model = _context.Resumes.FirstOrDefault();
            return View(model);
        }

        public IActionResult ApplyDetail()
        {
            var model = _context.Resumes.FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertCreateVM model)
        {
            try
            {
                var u = await _userManager.GetUserAsync(User);
                var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

                Advert advert = new();

                advert.ApplicationUserId = user!.Id;
                advert.Title = model.Title;
                advert.Description = model.Description;
                advert.Type = (AdvertType)model.Type;
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
                else if (model.Type == AdvertType.INTERN)
                {
                    advert.SquareMeter = model.SquareMeter;
                    advert.MonthlyTurnover = model.MonthlyTurnover;
                    advert.LicenseRightLeft = model.LicenseRightLeft;
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
