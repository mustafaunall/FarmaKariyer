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
}
