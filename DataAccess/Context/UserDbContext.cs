using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    // TODO: Add DbSets

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Resume>()
           .Property(d => d.ReferencePharmacies)
           .HasColumnType("jsonb");

        modelBuilder.Entity<Resume>()
           .Property(d => d.ForeignLanguages)
           .HasColumnType("jsonb");

        new UserDbSeed(modelBuilder).Seed();
    }
}
