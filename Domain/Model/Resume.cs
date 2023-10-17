using Domain.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model;

public class Resume : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string WorkExperience { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }

    public string SchoolName { get; set; }
    public string EducationStatus { get; set; }
    public string? EducationsAndCertificates { get; set; }

    [Column(TypeName = "jsonb")]
    public List<ReferencePharmacy>? ReferencePharmacies { get; set; }
    [Column(TypeName = "jsonb")]
    public List<ForeignLanguage>? ForeignLanguages { get; set; }

    public string? Description { get; set; }
    public string? ResumeFilePath { get; set; }

    #region MyRegion

    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    #endregion
}

public class ReferencePharmacy
{
    public string PharmacyName { get; set; }
    public string Position { get; set; }
    public string ExperienceYear { get; set; }
}

public class ForeignLanguage
{
    public string LanguageName { get; set; }
    public string Grade { get; set; }
}