using DataAccess.Context;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Extensions;
using WebUI.Models.ViewModels;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class ApplyController : BaseController
    {
        private readonly UserDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplyController(
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

            var applies = await _context.Applies
                .Include(x => x.ApplicantUser)
                .Include(x => x.CurrentResume)
                .Include(x => x.Advert)
                .Include(x => x.Advert.ApplicationUser)
                .Where(x => x.ApplicantUserId == user.Id)
                .ToListAsync();

            return View(applies);  
        }
    }
}
