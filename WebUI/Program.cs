using Core.Helper;
using Core.Services;
using DataAccess.Context;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PdfService>();

builder.Services.AddScoped<DbService>();

builder.Services.AddDbContext<UserDbContext>(options =>
options.UseNpgsql("host=80.253.246.121; port=5454; database=FarmaKariyer; username=postgres; password=Secure2023Password."), ServiceLifetime.Transient);

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

var resourcesPath = OsHelper.GetResourcesPath();

app.UseRouting();

app.UseSession();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "MyArea",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();
