using DataAccess.Model.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class AdminDbContext : IdentityDbContext<ApplicationAdminUser, IdentityRole<int>, int>
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
    {

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    // TODO: Add DbSets

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new AdminDbSeed(modelBuilder).Seed();
    }
}
