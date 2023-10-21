using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Domain.Model;

public class ApplicationUser : IdentityUser<int>
{
    [DefaultValue(false)]
    public bool IsAdmin { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }

    #region Pharmacy

    public string? PharmacyName { get; set; }
    public string? GLNCode { get; set; }
    public string? PharmacyType { get; set; }
    public string? EmployeeCount { get; set; }
    public string? SchoolName { get; set; }
    public string? EducationStatus { get; set; }
    public string? Certificates { get; set; }
    public string? Description { get; set; }

    #endregion

    public ApplicationUserType Type { get; set; }

    #region Relation

    public int? ActiveResumeId { get; set; }
    public List<Advert> Adverts { get; set; }

    #endregion
}

public enum ApplicationUserType
{
    USER,
    PHARMACY
}