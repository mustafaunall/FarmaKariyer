using Core.Helper;
using Core.Services;
using Core.Services.Hangfire.CheckAdvert;
using Core.Services.Hangfire.RecurringJobs;
using DataAccess.Context;
using Domain.Model;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Security.Authentication;
using WebUI.Operation;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    });
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PdfService>();

builder.Services.AddScoped<DbService>();

builder.Services.AddScoped<IRecurringJobs, RecurringJobs>();

builder.Services.AddScoped<IAdvertCheckJob, AdvertCheckJob>();

builder.Services.AddDbContext<UserDbContext>(options =>
options.UseNpgsql("host=160.20.108.234; port=5432; database=FarmaKariyer; username=postgres; password=pJn91Yn7WdjxKtjvZrAn"), ServiceLifetime.Transient);

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddHangfire(config =>
{
    var option = new PostgreSqlStorageOptions()
    {
        PrepareSchemaIfNecessary = true,
        QueuePollInterval = TimeSpan.FromSeconds(5),
        SchemaName = "job_prod"
    };
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UsePostgreSqlStorage("host=160.20.108.234; port=5432; database=FarmaKariyer; username=postgres; password=pJn91Yn7WdjxKtjvZrAn", option);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStatusCodePagesWithReExecute("/Error/ErrorHandler", "?statusCode={0}");
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

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always
    });

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireServer(new BackgroundJobServerOptions
{
    WorkerCount = 1,
});
//app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireDashboardAuthorizationFilter() } });
app.UseHangfireDashboard("/hangfire");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var jobService = services.GetRequiredService<IRecurringJobs>();
        jobService.CheckExpiredAdverts();
        jobService.CheckExpiredBoostedAdverts();

    }
    catch (Exception) { }
}

app.Run();
