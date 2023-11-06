using Domain.Model.Enum;

namespace WebUI.Models.ViewModels;

public class AdvertCreateVM
{
    public string Title { get; set; }
    public string Description { get; set; }
    public AdvertType Type { get; set; }
    public bool IsBoosted { get; set; }

    #region Technician

    public string ExperienceYear { get; set; }
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

    #region Intern

    public string EducationStatus { get; set; }

    #endregion

    #region Assistant

    public string AssistantExperienceYear { get; set; }

    #endregion

    #region License

    public string SquareMeter { get; set; }
    public int MonthlyTurnover { get; set; }
    public bool WithLicenseRight { get; set; }
    public bool WithoutLicenseRight { get; set; }
    public bool HasRightToCarry { get; set; }

    #endregion

    #region Other

    public List<string> DriverLicenses { get; set; }

    #endregion
}
