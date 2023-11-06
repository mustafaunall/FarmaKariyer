using Domain.Model;

namespace WebUI.Areas.Pharmacy.Models.ViewModels;

public class ApplicantProfileVM
{
    public Apply Apply { get; set; }
    public Advert Advert { get; set; }
    public ApplicationUser User { get; set; }
    public Resume Resume { get; set; }
}
