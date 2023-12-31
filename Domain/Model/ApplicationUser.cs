﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Domain.Model;

public class ApplicationUser : IdentityUser<int>
{
    [DefaultValue(false)]
    public bool IsAdmin { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? PhotoPath { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime RegisterDate { get; set; }

    #region Pharmacy

    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public int AdvertPostingQuota { get; set; } = 0;
    public string? PharmacyName { get; set; }
    public string? TaxOffice { get; set; }
    public string? TaxNumber { get; set; }
    public string? GLNCode { get; set; }
    public string? PharmacyType { get; set; }
    public string? EmployeeCount { get; set; }
    public string? SchoolName { get; set; }
    public string? EducationStatus { get; set; }
    public string? Certificates { get; set; }
    public string? Description { get; set; }

    #endregion

    public ApplicationUserType Type { get; set; }
    public string? PasswordResetCode { get; set; }

    #region Relation

    public int? ActiveResumeId { get; set; }
    public List<Advert> Adverts { get; set; }

    #endregion
}

public enum ApplicationUserType
{
    USER,
    PHARMACY
}