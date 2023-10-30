using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User;

public class UpdateProfileVM
{
    //public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public string? PharmacyType { get; set; }
    public string? EmployeeCount { get; set; }
    public string? SchoolName { get; set; }
    public string? EducationStatus { get; set; }
    public string? Certificates { get; set; }
    public string? Description { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? NewPasswordRepeat { get; set; }
    public int RequirePassword { get; set; } = 0;
}
