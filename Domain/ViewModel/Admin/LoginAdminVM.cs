using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Admin;

public class LoginAdminVM
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
