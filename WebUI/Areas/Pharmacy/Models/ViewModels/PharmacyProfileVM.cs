using Domain.Model;

namespace WebUI.Areas.Pharmacy.Models.ViewModels;

public class PharmacyProfileVM
{
    public ApplicationUser User { get; set; }
    public int AdvertCount { get; set; } = 0;
    public int TotalApplyCount { get; set; } = 0;
    public int AverageApplyCount { get; set; } = 0;
}
