using DataAccess.Context;
using Domain.Model;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertCreateVM model)
        {
            try
            {
                var u = await _userManager.GetUserAsync(User);
                var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

                Advert advert = new();

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
                    advert.MonthlyTurnover
                }

                await _context.AddAsync(advert);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    //user.ResumeId = advert.Id;
                    await _context.SaveChangesAsync();
                    Notification("İlan başarıyla yayınlandı.", NotificationType.Success);
                    return Redirect("/User/Advert/List");
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
