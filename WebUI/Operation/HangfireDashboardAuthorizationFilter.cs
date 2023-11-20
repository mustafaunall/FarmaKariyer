using Hangfire.Dashboard;

namespace WebUI.Operation;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;

        //var httpContext = context.GetHttpContext();

        //// Allow all authenticated users to see the Dashboard (potentially dangerous).
        //return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}
