﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.ViewModel.User;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using Core.Helper;
using Domain.Model;
using WebUI.Attributes;
using WebUI.Extensions;
using WebUI.Areas.User.Models.ViewModels;

namespace WebUI.Areas.User.Controllers;

[Area("User")]
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
            var resume = await _context.Resumes
                .Where(x => x.Id == user.ActiveResumeId)
                .FirstOrDefaultAsync();
            var applyCount = await _context.Applies
                .Where(x => x.ApplicantUserId == user.Id)
                .CountAsync();
            var resumeCount = await _context.Resumes
                .Where(x => x.ApplicationUserId == user.Id)
                .CountAsync();
            return View(new UserProfileVM()
            {
                User = user,
                Resume = resume,
                ApplyCount = applyCount,
                ResumeCount = resumeCount,
            });
        }
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Packages()
    {
        if (User.Identity.IsAuthenticated)
        {
            var u = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
                .SingleAsync(x => x.Email == u.Email);
            if (user == null)
            {
                return Redirect("/Account/LoginPharmacy");
            }
            if (user.Type == ApplicationUserType.PHARMACY)
            {
                return Redirect("/Pharmacy/Account/Packages");
            }
            else if (user.Type == ApplicationUserType.USER)
            {
                Notification("Bu işlem yalnızca işveren hesabı ile gerçekleştirilebilir!", NotificationType.Info);
                return Redirect("/Home/Index");
            }
        }
        return Redirect("/Account/LoginPharmacy");
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
        if (string.IsNullOrEmpty(vm.CurrentPassword))
        {
            Notification("Bilgileriniz güncellenemedi, lütfen şu anki şifrenizi giriniz!", NotificationType.Info);
            return RedirectToAction(nameof(Profile));
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (!(await _userManager.CheckPasswordAsync(user, vm.CurrentPassword)))
            {
                Notification("Bilgilerinizi güncellemek için lütfen şifrenizi doğru giriniz!", NotificationType.Info);
                return RedirectToAction(nameof(Profile));
            }
            if (!string.IsNullOrEmpty(vm.Name))
                user.Name = vm.Name.Trim();
            if (!string.IsNullOrEmpty(vm.Surname))
                user.Surname = vm.Surname.Trim();
            if (!string.IsNullOrEmpty(vm.PhoneNumber))
                user.PhoneNumber = vm.PhoneNumber.Trim();
            if (!string.IsNullOrEmpty(vm.Email))
                user.Email = vm.Email.Trim();
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

            Type = ApplicationUserType.USER,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectPermanent("/User/Account/Profile");
        }
        else
        {
            Notification("Kayıt işleminde bir hata oluştu", NotificationType.Error);
            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}