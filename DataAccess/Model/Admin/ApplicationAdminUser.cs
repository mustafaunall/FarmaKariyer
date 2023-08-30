﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace DataAccess.Model.Admin;

public class ApplicationAdminUser : IdentityUser<int>
{
    [DefaultValue(false)]
    public bool IsAdmin { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Address { get; set; }
}
