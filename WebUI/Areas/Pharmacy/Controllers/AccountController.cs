using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.ViewModel.User;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Core.Helper;
using Domain.Model;
using WebUI.Attributes;
using WebUI.Extensions;
using static SkiaSharp.HarfBuzz.SKShaper;
using WebUI.Areas.User.Models.ViewModels;
using WebUI.Areas.Pharmacy.Models.ViewModels;

namespace WebUI.Areas.Pharmacy.Controllers;

[Area("Pharmacy")]
public class AccountController : BaseController
{
    private readonly UserDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, UserDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            var u = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
                .SingleAsync(x => x.Email == u.Email);
            var advertCount = await _context.Adverts
                .Where(x => x.ApplicationUserId == user.Id)
                .CountAsync();
            var totalApplyCount = await _context.Applies
                .Include(x => x.Advert)
                .Where(x => x.Advert.ApplicationUserId == user.Id)
                .CountAsync();
            var averageApplyCount = totalApplyCount / advertCount;
            return View(new PharmacyProfileVM()
            {
                User = user,
                AdvertCount = advertCount,
                AverageApplyCount = averageApplyCount,
                TotalApplyCount = totalApplyCount,
            });
        }
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileVM vm)
    {
        if (!ModelState.IsValid || vm == null)
        {
            Notification("Lütfen tüm alanları doldurunuz!", NotificationType.Info);
            return RedirectToAction(nameof(Profile));
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(vm.Name))
                user.Name = vm.Name.Trim();
            if (!string.IsNullOrEmpty(vm.Surname))
                user.Surname = vm.Surname.Trim();
            if (!string.IsNullOrEmpty(vm.PhoneNumber))
                user.PhoneNumber = vm.PhoneNumber.Trim();
            if (!string.IsNullOrEmpty(vm.Email))
                user.Email = vm.Email.Trim();
            if (!string.IsNullOrEmpty(vm.Province))
                user.Province = vm.Province.Trim();
            if (!string.IsNullOrEmpty(vm.District))
                user.District = vm.District.Trim();
            if (!string.IsNullOrEmpty(vm.Address))
                user.Address = vm.Address.Trim();
            if (!string.IsNullOrEmpty(vm.PharmacyType))
                user.PharmacyType = vm.PharmacyType.Trim();
            if (!string.IsNullOrEmpty(vm.EmployeeCount))
                user.EmployeeCount = vm.EmployeeCount.Trim();
            if (!string.IsNullOrEmpty(vm.SchoolName))
                user.SchoolName = vm.SchoolName.Trim();
            if (!string.IsNullOrEmpty(vm.EducationStatus))
                user.EducationStatus = vm.EducationStatus.Trim();
            if (!string.IsNullOrEmpty(vm.Certificates))
                user.Certificates = vm.Certificates.Trim();
            if (!string.IsNullOrEmpty(vm.Description))
                user.Description = vm.Description.Trim();
            await _userManager.UpdateAsync(user);
            Notification("Kullanıcı bilgileri başarıyla güncellendi.", NotificationType.Success);
            return RedirectToAction(nameof(Profile));
        }
        catch (Exception)
        {
            Notification("Kullanıcı bilgileri güncellenirken bir hata oluştu!", NotificationType.Error);
            return RedirectToAction(nameof(Profile));
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordVM vm)
    {
        if (!ModelState.IsValid || vm == null && (vm?.New?.Trim() != vm?.NewAgain?.Trim()))
        {
            return RedirectPermanent("/Account/Profile");
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (!(await _userManager.CheckPasswordAsync(user, vm.Current)))
            {
                ModelState.AddModelError("CurrentPassword", "Mevcut şifreniz yanlış.");
                return RedirectPermanent("/Account/Profile");
            }

            // Yeni şifreyi değiştirin
            await _userManager.ChangePasswordAsync(user, vm.Current, vm.New);

            // Kullanıcıyı yeniden oturum açmaya zorla
            await _signInManager.SignOutAsync();
            return RedirectPermanent("/Account/Profile");
        }
        catch (Exception)
        {
            return RedirectPermanent("/Account/Profile");
        }
    }

    [CheckSession]
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Packages()
    {
        return View();
    }

    public IActionResult LoginPharmacy()
    {
        return View();
    }
    public IActionResult RegisterPharmacy()
    {
        return View();
    }
    public IActionResult CV()
    {
        return View();
    }

    public IActionResult TechnicianCV()
    {
        return View();
    }
    public IActionResult InternCV()
    {
        return View();
    }
    public IActionResult PharmacyJobAdvert()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [CheckSession]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserVM model)
    {
        var user = new ApplicationUser
        {
            UserName = StringHelper.ConvertToEnglish(model.Name + model.Surname).ToLower(),
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Name = model.Name,
            Surname = model.Surname,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("Profile", "Account");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}