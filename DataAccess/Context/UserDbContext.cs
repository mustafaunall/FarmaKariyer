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

    public DbSet<AdvertCategory> AdvertCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Advert> Adverts { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Apply> Applies { get; set; }

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
