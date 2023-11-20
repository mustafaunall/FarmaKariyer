using Core.Services.Hangfire.CheckAdvert;
using Hangfire;
using System.Runtime.InteropServices;

namespace Core.Services.Hangfire.RecurringJobs;

public class RecurringJobs : IRecurringJobs
{
    TimeZoneInfo? turkeyZone = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        ? TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")
        : (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        ? TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul")
        : TimeZoneInfo.Utc;

    public RecurringJobs()
    {
    }

    [Obsolete]
    public bool CheckExpiredAdverts()
    {
        try
        {
            DateTime startDate = DateTime.Now;              // Job'un Başlangıç tarihi = Bugün
            DateTime endDate = startDate.AddYears(1);       // Job'un Bitiş Tarihi     = Bir yıl sonra
            string jobId = @$"CHECK_EXPIRED_ADVERTS_JOB";   // Job'un ID'si
            string cron = "30 23 * * *";                    // Job'un CRON değeri (Her gün 23:30 saatinde çalışacak)
            //string cron = "* * * * *";                    // Job'un CRON test değeri (Her saniye çalışacak) (Test için)

            RecurringJob.AddOrUpdate<IAdvertCheckJob>(jobId, job => job.CheckExpiredAdverts(), cronExpression: cron, timeZone: turkeyZone);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [Obsolete]
    public bool CheckExpiredBoostedAdverts()
    {
        try
        {
            DateTime startDate = DateTime.Now;              // Job'un Başlangıç tarihi = Bugün
            DateTime endDate = startDate.AddYears(1);       // Job'un Bitiş Tarihi     = Bir yıl sonra
            string jobId = @$"CHECK_EXPIRED_BOOSTED_ADVERTS_JOB";   // Job'un ID'si
            string cron = "30 23 * * *";                    // Job'un CRON değeri (Her gün 23:30 saatinde çalışacak)
            //string cron = "* * * * *";                    // Job'un CRON test değeri (Her saniye çalışacak) (Test için)

            RecurringJob.AddOrUpdate<IAdvertCheckJob>(jobId, job => job.CheckExpiredBoostedAdverts(), cronExpression: cron, timeZone: turkeyZone);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
