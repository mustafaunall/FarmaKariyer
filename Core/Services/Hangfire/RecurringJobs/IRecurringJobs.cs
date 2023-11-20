namespace Core.Services.Hangfire.RecurringJobs;

public interface IRecurringJobs
{
    bool CheckExpiredAdverts();
    bool CheckExpiredBoostedAdverts();
}
