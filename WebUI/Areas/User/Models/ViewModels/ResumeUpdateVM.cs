namespace WebUI.Models.ViewModels;

public class ResumeUpdateVM
{
    public string Phone { get; set; }
    public string Email { get; set; }
    public string WorkExperience { get; set; }
    public DateTime BirthDate { get; set; }

    public string SchoolName { get; set; }
    public string EducationStatus { get; set; }
    public string? EducationsAndCertificates { get; set; }

    public List<string>? ReferencePharmacyNames { get; set; }
    public List<string>? ReferencePharmacyPositions { get; set; }
    public List<string>? ReferencePharmacyExperienceYears { get; set; }

    public List<string>? ForeignLanguageNames { get; set; }
    public List<string>? ForeignLanguageGrades { get; set; }

    public string? Description { get; set; }
    public IFormFile? ResumeFile { get; set; }
}
