using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.ViewModel.User;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Model.User;

namespace WebUI.Controllers;

public class AccountController : Controller
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
    public IActionResult Index()
    {
        return RedirectPermanent("/Account/Profile");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            var u = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
                .SingleAsync(x => x.Email == u.Email);
            return View(user);
        }
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileVM vm)
    {
        if (!ModelState.IsValid || vm == null)
        {
            return RedirectPermanent("/Account/Profile");
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(vm.UserName))
                user.UserName = vm.UserName.Trim();
            if (!string.IsNullOrEmpty(vm.Name))
                user.Name = vm.Name.Trim();
            if (!string.IsNullOrEmpty(vm.Surname))
                user.Surname = vm.Surname.Trim();
            if (!string.IsNullOrEmpty(vm.PhoneNumber))
                user.PhoneNumber = vm.PhoneNumber.Trim();
            if (!string.IsNullOrEmpty(vm.Email))
                user.Email = vm.Email.Trim();
            await _userManager.UpdateAsync(user);
            return RedirectPermanent("/Account/Profile");
        }
        catch (Exception)
        {
            return RedirectPermanent("/Account/Profile");
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

    public IActionResult Login()
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
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
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
        }

        return RedirectToActionPermanent("Login", model);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}