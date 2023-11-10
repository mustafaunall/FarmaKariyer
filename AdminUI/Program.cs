using DataAccess.Context;
using Domain.Model;
using DotNetEd.CoreAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UserDbContext>(options =>
options.UseNpgsql("host=160.20.108.234; port=5432; database=FarmaKariyer; username=postgres; password=pJn91Yn7WdjxKtjvZrAn"));

builder.Services.AddCoreAdmin(new CoreAdminOptions()
{
    IgnoreEntityTypes = new List<Type>() {
        typeof(IdentityUser)
    }
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute();

#region CoreAdmin

app.UseCoreAdminCustomUrl("/yonetim");
app.UseCoreAdminCustomTitle("Avrupa Card Yönetim Paneli");

#endregion

app.Run();
