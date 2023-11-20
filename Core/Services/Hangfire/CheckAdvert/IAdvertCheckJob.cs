namespace Core.Services.Hangfire.CheckAdvert;

public interface IAdvertCheckJob
{
    Task CheckExpiredAdverts();
    Task CheckExpiredBoostedAdverts();
}
