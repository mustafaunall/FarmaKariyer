using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class LoginUserVM
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
