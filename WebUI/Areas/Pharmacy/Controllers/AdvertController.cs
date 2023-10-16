using DataAccess.Context;
using Domain.Model;
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

                Resume resume = new();

                // TODO: İlan ekleme komutları eklenecek

                await _context.AddAsync(resume);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    user.ResumeId = resume.Id;
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
