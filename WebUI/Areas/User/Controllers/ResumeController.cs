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
    public class ResumeController : BaseController
    {
        private readonly UserDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResumeController(
            UserDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Update()
        {
            var u = await _userManager.GetUserAsync(User);
            var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();
            if (user!.ActiveResumeId == null)
                return View();

            var resume = _context.Resumes.Where(x => x.Id == user.ActiveResumeId).FirstOrDefault();
            if (resume == null)
                return View();
            ResumeUpdateVM vm = new()
            {
                Name = resume.Name,
                Surname = resume.Surname,
                Phone = resume.Phone,
                Email = resume.Email,
                WorkExperience = resume.WorkExperience,
                BirthDate = resume.BirthDate,
                Address = resume.Address,
                SchoolName = resume.SchoolName,
                EducationStatus = resume.EducationStatus,
                EducationsAndCertificates = resume.EducationsAndCertificates,
                Description = resume.Description,

                ReferencePharmacyNames = resume.ReferencePharmacies?.Select(x => x.PharmacyName).ToList(),
                ReferencePharmacyPositions = resume.ReferencePharmacies?.Select(x => x.Position).ToList(),
                ReferencePharmacyExperienceYears = resume.ReferencePharmacies?.Select(x => x.ExperienceYear).ToList(),

                ForeignLanguageNames = resume.ForeignLanguages?.Select(x => x.LanguageName).ToList(),
                ForeignLanguageGrades = resume.ForeignLanguages?.Select(x => x.Grade).ToList(),
            };
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Update(ResumeUpdateVM model)
        {
            try
            {
                var u = await _userManager.GetUserAsync(User);
                var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

                Resume resume = new();

                resume.Name = model.Name;
                resume.Surname = model.Surname;
                resume.Phone = model.Phone;
                resume.Email = model.Email;
                resume.WorkExperience = model.WorkExperience;
                resume.BirthDate = model.BirthDate;
                resume.Address = model.Address;
                resume.SchoolName = model.SchoolName;
                resume.EducationStatus = model.EducationStatus;
                if (!string.IsNullOrEmpty(model.EducationsAndCertificates))
                    resume.EducationsAndCertificates = model.EducationsAndCertificates;
                if (!string.IsNullOrEmpty(model.Description))
                    resume.Description = model.Description;

                List<ReferencePharmacy> referencePharmacies = new();
                if (model.ReferencePharmacyNames != null)
                {
                    for (int i = 0; i < model.ReferencePharmacyNames.Count; i++)
                    {
                        ReferencePharmacy referencePharmacy = new();
                        referencePharmacy.PharmacyName = model.ReferencePharmacyNames.ElementAt(i);
                        referencePharmacy.Position = model.ReferencePharmacyPositions!.ElementAt(i);
                        referencePharmacy.ExperienceYear = model.ReferencePharmacyExperienceYears!.ElementAt(i);
                        referencePharmacies.Add(referencePharmacy);
                    }
                }
                resume.ReferencePharmacies = referencePharmacies;

                List<ForeignLanguage> foreignLanguages = new();
                if (model.ForeignLanguageNames != null)
                {
                    for (int i = 0; i < model.ForeignLanguageNames.Count; i++)
                    {
                        ForeignLanguage foreignLanguage = new();
                        foreignLanguage.LanguageName = model.ForeignLanguageNames.ElementAt(i);
                        foreignLanguage.Grade = model.ForeignLanguageGrades!.ElementAt(i);
                        foreignLanguages.Add(foreignLanguage);
                    }
                }
                resume.ForeignLanguages = foreignLanguages;

                resume.ApplicationUserId = user!.Id;

                await _context.AddAsync(resume);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    user.ActiveResumeId = resume.Id;
                    await _context.SaveChangesAsync();
                    Notification("Özgeçmiş başarıyla kaydedildi", NotificationType.Success);
                    return Redirect("/User/Resume/Update");
                }
            }
            catch (Exception ex)
            {
                Notification("Özgeçmiş güncellenirken bir hata meydana geldi!", NotificationType.Error);
                return View(model);
            }
            return View(model);
        }
    }
}
