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
using static IdentityServer4.Models.IdentityResources;

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
            var averageApplyCount = advertCount == 0 ? 0 : (totalApplyCount / advertCount);
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
    public async Task<IActionResult> UpdatePhoto(IFormFile Photo)
    {
        if (Photo == null)
        {
            Notification("Lütfen fotoğraf seçiniz!", NotificationType.Info);
            return Redirect("/Pharmacy/Account/Profile");
        }

        try
        {
            if (!Directory.Exists(OsHelper.GetPhotoFilesPath()))
            {
                Directory.CreateDirectory(OsHelper.GetPhotoFilesPath());
            }

            var u = await _userManager.GetUserAsync(User);
            var user = await _context.Users.Where(x => x.Id == u.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                Notification($"Kullanıcı bulunamadı!", NotificationType.Error);
                return Redirect("/Pharmacy/Account/Profile");
            }

            var fileName = Photo?.FileName;
            var extension = Path.GetExtension(Path.Combine(OsHelper.GetPhotoFilesPath(), fileName));
            string guidFileName = $"{DateTime.Now.Ticks.ToString()}{extension}";
            var uploadPath = Path.Combine(OsHelper.GetPhotoFilesPath(), guidFileName);
            var stream = new FileStream(uploadPath, FileMode.Create);
            await Photo!.CopyToAsync(stream);
            user.PhotoPath = guidFileName;
            await _context.SaveChangesAsync();

            Notification($"Sayın {user.Name} {user.Surname}, profil fotoğrafınız başarıyla güncellendi.", NotificationType.Success);
            return Redirect("/Pharmacy/Account/Profile");
        }
        catch (Exception)
        {
            Notification("Bir hata meydana geldi!", NotificationType.Error);
            return Redirect("/Pharmacy/Account/Profile");
        }
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
        var user = await _userManager.GetUserAsync(User);
        if (vm.RequirePassword == 1)
        {
            if (string.IsNullOrEmpty(vm.CurrentPassword))
            {
                Notification("Bilgileriniz güncellenemedi, lütfen şu anki şifrenizi giriniz!", NotificationType.Info);
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                if (!(await _userManager.CheckPasswordAsync(user, vm.CurrentPassword)))
                {
                    Notification("Bilgilerinizi güncellemek için lütfen şifrenizi doğru giriniz!", NotificationType.Info);
                    return RedirectToAction(nameof(Profile));
                }
            }
        }

        try
        {
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
            if (!string.IsNullOrEmpty(vm.Latitude))
                user.Latitude = vm.Latitude.Trim();
            if (!string.IsNullOrEmpty(vm.Longitude))
                user.Longitude = vm.Longitude.Trim();
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
            string notificationValue = "Kullanıcı bilgileri başarıyla güncellendi.";
            if (!string.IsNullOrEmpty(vm.NewPassword) && !string.IsNullOrEmpty(vm.NewPasswordRepeat))
            {
                if (vm.NewPassword != vm.NewPasswordRepeat)
                {
                    Notification("Şifreler eşleşmiyor, lütfen tekrar kontrol ediniz!", NotificationType.Error);
                    return RedirectToAction(nameof(Profile));
                }
                await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
                notificationValue += " Şifreniz güncellendiği için tekrar giriş yapmanız gerekmektedir.";
                await _signInManager.SignOutAsync();
            }
            Notification(notificationValue, NotificationType.Success);
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
    public async Task<IActionResult> Packages()
    {
        var u = await _userManager.GetUserAsync(User);
        var user = await _userManager.Users
            .SingleAsync(x => x.Email == u.Email);
        if (user.AdvertPostingQuota == -1)
        {
            Notification("Şu an zaten yıllık paket kullanıyorsunuz!", NotificationType.Info);
            return Redirect("/Pharmacy/Account/Profile");
        }
        return View(new PackagesVM()
        {
            PackagePrice1 = _context.AdvertCategories.Where(x => x.Id == 1).Select(x => x.Price).FirstOrDefault(),
            PackagePrice2 = _context.AdvertCategories.Where(x => x.Id == 2).Select(x => x.Price).FirstOrDefault(),
            PackagePrice3 = _context.AdvertCategories.Where(x => x.Id == 3).Select(x => x.Price).FirstOrDefault(),
        });
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