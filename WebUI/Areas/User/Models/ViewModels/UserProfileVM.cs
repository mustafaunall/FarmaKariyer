using Domain.Model;

namespace WebUI.Areas.User.Models.ViewModels;

public class UserProfileVM
{
    public ApplicationUser User { get; set; }
    public Resume? Resume { get; set; }
    public int ApplyCount { get; set; } = 0;
    public int ResumeCount { get; set; } = 0;
}
