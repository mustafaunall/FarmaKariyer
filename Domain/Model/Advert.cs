using Domain.Model.Base;
using Domain.Model.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model;

public class Advert : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public AdvertType Type { get; set; }
    public bool IsBoosted { get; set; } = false;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    #region Technician

    public string? ExperienceYear { get; set; }
    public bool BonusBenefit { get; set; }
    public bool TravelBenefit { get; set; }
    public bool FoodBenefit { get; set; }
    public int SalaryRange { get; set; }
    public bool PrescriptionInfo { get; set; }
    public bool PrivateInsuranceEntryInfo { get; set; }
    public bool OTCInfo { get; set; }
    public bool DermocosmeticInfo { get; set; }
    public string? OtherInfo { get; set; }

    #endregion

    #region Dermocosmetic

    public bool IsDermocosmetic { get; set; } = false;

    #endregion

    #region Intern

    public string? EducationStatus { get; set; }

    #endregion

    #region Assistant

    // İş Deneyimi (yıl)

    #endregion

    #region License

    public string? SquareMeter { get; set; }
    public int MonthlyTurnover { get; set; }
    public bool WithLicenseRight { get; set; }
    public bool WithoutLicenseRight { get; set; }
    public bool HasRightToCarry { get; set; }

    #endregion

    #region Other

    [Column(TypeName = "jsonb")]
    public List<string>? DriverLicenses { get; set; }

    #endregion

    #region Relation

    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    #endregion
}
