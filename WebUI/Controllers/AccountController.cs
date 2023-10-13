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

namespace WebUI.Controllers;

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

    [CheckSession]
    public IActionResult LoginUser()
    {
        return View();
    }

    [CheckSession]
    public IActionResult LoginPharmacy()
    {
        return View();
    }

    [CheckSession]
    public IActionResult RegisterUser()
    {
        return View();
    }

    [CheckSession]
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
                if (user.Type == ApplicationUserType.USER)
                    return RedirectToAction("Profile", "Account", new { area = "User" });
                else if (user.Type == ApplicationUserType.PHARMACY)
                    return RedirectToAction("Profile", "Account", new { area = "Pharmacy" });
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
    public async Task<IActionResult> RegisterUser(RegisterUserVM model)
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

            return RedirectToAction("Index", "Home");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("Profile", "Account");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterPharmacy(RegisterUserVM model)
    {
        var user = new ApplicationUser
        {
            UserName = StringHelper.ConvertToEnglish(model.Name + model.Surname).ToLower(),
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Name = model.Name,
            Surname = model.Surname,
            Type = ApplicationUserType.PHARMACY,
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