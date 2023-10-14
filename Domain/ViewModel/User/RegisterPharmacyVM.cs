﻿using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class RegisterPharmacyVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string PharmacyName { get; set; }

    [Required]
    public string GLNCode { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}