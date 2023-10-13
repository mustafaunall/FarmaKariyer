using DataAccess.Context;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Services;

public class DbService
{
    private readonly UserDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public DbService(
        UserDbContext context,
        UserManager<ApplicationUser> userManager
        )
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetSessionUser(ClaimsPrincipal User)
    {
        var u = await _userManager.GetUserAsync(User);
        var user = _context.Users.Where(x => x.Id == u.Id).FirstOrDefault();

        return user;
    }
}
