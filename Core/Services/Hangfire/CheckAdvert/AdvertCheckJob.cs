using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Hangfire.CheckAdvert;

public class AdvertCheckJob : IAdvertCheckJob
{
    private readonly UserDbContext _context;

    public AdvertCheckJob(UserDbContext context)
    {
        _context = context;
    }

    public async Task CheckExpiredAdverts()
    {
        try
        {
            var expiredAdverts = await _context.Adverts
                .Where(x => DateTime.Now > x.CreateDate.AddDays(30)).ToListAsync();
            if (expiredAdverts != null && expiredAdverts.Any())
            {
                foreach (var advert in expiredAdverts)
                {
                    advert.IsActive = false;
                }
                _context.Adverts.UpdateRange(expiredAdverts);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw new Exception("Error at Checking Expired Adverts");
        }
    }

    public async Task CheckExpiredBoostedAdverts()
    {
        try
        {
            var expiredBoostedAdverts = await _context.Adverts
                .Where(x => x.IsBoosted == true && x.BoostCreateDate != null).ToListAsync();
            expiredBoostedAdverts = expiredBoostedAdverts.Where(x => DateTime.Now > ((DateTime)x.BoostCreateDate!).AddDays(7)).ToList();
            if (expiredBoostedAdverts != null && expiredBoostedAdverts.Any())
            {
                foreach (var advert in expiredBoostedAdverts)
                {
                    advert.IsBoosted = false;
                    advert.BoostCreateDate = null;
                }
                _context.Adverts.UpdateRange(expiredBoostedAdverts);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw new Exception("Error at Checking Expired Boosted Adverts");
        }
    }
}
