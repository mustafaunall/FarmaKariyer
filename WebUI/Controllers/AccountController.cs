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
using Core.Services;
using System.Text.Encodings.Web;

namespace WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly UserDbContext _context;
    private readonly MailService _mailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, UserDbContext context, MailService mailService)
    {
        _mailService = mailService;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Profile()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        var u = await _userManager.GetUserAsync(User);
        var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

        if (user == null)
            return RedirectToAction("Index", "Home");

        if (user.Type == ApplicationUserType.USER)
            return RedirectPermanent("/User/Account/Profile");
        else if (user.Type == ApplicationUserType.PHARMACY)
            return RedirectPermanent("/Pharmacy/Account/Profile");
        else
            return RedirectToAction("Index", "Home");
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
    public IActionResult ResetPassword()
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
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var user = _context.Users.Where(x => x.Email == vm.Email).FirstOrDefault();
        if (user == null)
        {
            Notification("Kullanıcı bulunamadı!", NotificationType.Error, "Hata");
            return View(vm);
        }
        string resetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
        user.PasswordResetCode = resetCode;
        await _context.SaveChangesAsync();
        _mailService.SendMail(user.Email, "Şifre Sıfırlama Talebi", $@"
Merhabalar,

FarmaKariyer.net üzerinden gerçekleştirdiğiniz şifre sıfırlama talebi tarafımıza iletilmiş olup, şifre sıfırlama bağlantı adresiniz aşağıda bulunmaktadır.
Bu işlem sizin tarafınızdan gerçekleşmediyse bağlantı adresine giriş sağlamamanızı tavsiye ediyoruz.

https://farmakariyer.net/Account/ResetPassword?email={user.Email}&code={UrlEncoder.Default.Encode(resetCode)}


İyi günler dileriz.
            ");
        Notification("Şifre sıfırlama talebiniz alınmıştır, epostanızı kontrol ediniz.", NotificationType.Success, "Şifre Sıfırlama Başarılı");
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string code)
    {
        var user = _context.Users.Where(x => x.Email == email).FirstOrDefault();
        if (user == null)
        {
            Notification("Kullanıcı bulunamadı!", NotificationType.Error, "Hata");
            return RedirectPermanent("/Account/ForgotPassword");
        }
        if (user.PasswordResetCode != code)
        {
            Notification("Şifre sıfırlama kodu yanlış!", NotificationType.Error, "Hata");
            return RedirectPermanent("/Account/ForgotPassword");
        }
        return View(new ResetPasswordVM()
        {
            Email = email,
            Code = code,
        });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
    {
        var user = await _userManager.FindByEmailAsync(vm.Email);
        //var user = _context.Users.Where(x => x.Email == vm.Email).AsNoTracking().FirstOrDefault();
        if (user == null)
        {
            Notification("Kullanıcı bulunamadı!", NotificationType.Error, "Hata");
            return View(vm);
        }
        if (vm.Password != vm.ConfirmPassword)
        {
            Notification("Şifreler aynı olmalıdır!", NotificationType.Error, "Hata");
            return View(vm);
        }
        if (string.IsNullOrEmpty(vm.Password) || string.IsNullOrEmpty(vm.ConfirmPassword))
        {
            Notification("Şifreler boş girilemez!", NotificationType.Error, "Hata");
            return View(vm);
        }
        if (user.PasswordResetCode != vm.Code)
        {
            Notification("Şifre sıfırlama kodu yanlış!", NotificationType.Error, "Hata");
            return View(vm);
        }
        var result = await _userManager.ResetPasswordAsync(user, vm.Code, vm.Password);
        if (result.Succeeded == false)
        {
            Notification("Şifre sıfırlanırken bir hata oluştu!", NotificationType.Error, "Hata");
            return View(vm);
        }
        user.PasswordResetCode = null;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        Notification("Şifreniz başarıyla güncellendi, şimdi giriş yapabilirsiniz.", NotificationType.Success, "Şifre Değiştirme Başarılı");
        return RedirectPermanent("/Account/LoginUser");
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
            if ((int)user.Type != model.UserType)
            {
                if (model.UserType == (int)ApplicationUserType.PHARMACY)
                {
                    Notification("İşveren girişine yetkiniz bulunamadı, iş arayan girişine yönlendiriliyorsunuz...", NotificationType.Info);
                    return RedirectToAction("LoginUser", "Account");
                }
                else if (model.UserType == (int)ApplicationUserType.USER)
                {
                    Notification("İş arayan girişine yetkiniz bulunamadı, işveren girişine yönlendiriliyorsunuz...", NotificationType.Info);
                    return RedirectToAction("LoginPharmacy", "Account");
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                if (user.Type == ApplicationUserType.USER && user.ActiveResumeId == null)
                {
                    Notification("Farma Kariyer'e hoş geldiniz, şimdi özgeçmişinizi oluşturabilirsiniz.", NotificationType.Info);
                }

                if (user.Type == ApplicationUserType.USER)
                    return RedirectPermanent("/User/Account/Profile");
                else if (user.Type == ApplicationUserType.PHARMACY)
                    return RedirectPermanent("/Pharmacy/Account/Profile");
            }
            else
            {
                Notification("E-postanız veya şifreniz yanlış, lütfen tekrar deneyin!", NotificationType.Error);
                if (model.UserType == (int)ApplicationUserType.PHARMACY)
                    return RedirectToAction("LoginPharmacy", "Account");
                else if (model.UserType == (int)ApplicationUserType.USER)
                    return RedirectToAction("LoginUser", "Account");
            }
        }
        Notification("Kullanıcı bulunamadı", NotificationType.Error);
        return RedirectToAction("LoginUser", "Account");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterUser(RegisterUserVM model)
    {
        try
        {
            if (model.BirthDateDay == 0 || model.BirthDateMonth == 0 || model.BirthDateYear == 0)
            {
                Notification("Doğum tarihi yanlış!", NotificationType.Error);
                return View(model);
            }
            var birthDate = new DateTime(model.BirthDateYear, model.BirthDateMonth, model.BirthDateDay);
            var user = new ApplicationUser
            {
                //UserName = StringHelper.ConvertToEnglish(model.Name + model.Surname).ToLower(),
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                Surname = model.Surname,
                BirthDate = birthDate,
                RegisterDate = DateTime.Now,
                Type = ApplicationUserType.USER,

                Province = model.Province,
                District = model.District,
                Address = model.Address,
            };
            var mailCheck = await _userManager.FindByEmailAsync(model.Email);
            if (mailCheck != null)
            {
                Notification("Bu e-posta adresi sistemimizde zaten kayıtlı!", NotificationType.Info);
                return View(model);
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                Notification("Farma Kariyer'e hoş geldiniz, şimdi özgeçmişinizi oluşturabilirsiniz.", NotificationType.Info);
                return RedirectPermanent("/User/Account/Profile");
            }
            return View(model);
        }
        catch (Exception)
        {
            Notification("Kayıt olma işlemi esnasında bir hata meydana geldi!", NotificationType.Error);
            return View(model);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterPharmacy(RegisterPharmacyVM model)
    {
        try
        {
            if (model.BirthDateDay == 0 || model.BirthDateMonth == 0 || model.BirthDateYear == 0)
            {
                Notification("Doğum tarihi yanlış!", NotificationType.Error);
                return View(model);
            }
            var birthDate = new DateTime(model.BirthDateYear, model.BirthDateMonth, model.BirthDateDay);
            var user = new ApplicationUser
            {
                //UserName = StringHelper.ConvertToEnglish(model.Name + model.Surname).ToLower(),
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                Surname = model.Surname,
                BirthDate = birthDate,
                RegisterDate = DateTime.Now,
                Type = ApplicationUserType.PHARMACY,

                PharmacyName = model.PharmacyName,
                TaxOffice = model.TaxOffice,
                TaxNumber = model.TaxNumber,
                GLNCode = model.GLNCode,

                Province = model.Province,
                District = model.District,
                Address = model.Address,
            };
            var mailCheck = await _userManager.FindByEmailAsync(model.Email);
            if (mailCheck != null)
            {
                Notification("Bu e-posta adresi sistemimizde zaten kayıtlı!", NotificationType.Info);
                return View(model);
            }
            var glnCheck = await _context.Users.Where(x => x.GLNCode == model.GLNCode).FirstOrDefaultAsync();
            if (glnCheck != null)
            {
                Notification("Bu GLN numarası sistemimizde zaten kayıtlı!", NotificationType.Info);
                return View(model);
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                Notification("Farma Kariyer'e hoş geldiniz, şimdi ilan verebilirsiniz.", NotificationType.Info);
                return RedirectPermanent("/Pharmacy/Account/Profile");
            }
            return View(model);
        }
        catch (Exception)
        {
            Notification("Kayıt olma işlemi esnasında bir hata meydana geldi!", NotificationType.Error);
            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LoginUser", "Account");
    }
}