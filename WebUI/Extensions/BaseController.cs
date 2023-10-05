using Microsoft.AspNetCore.Mvc;

namespace WebUI.Extensions;

public enum NotificationType
{
    Success,
    Error,
    Info
}

public class BaseController : Controller
{
    public void Notification(string msg, NotificationType type = NotificationType.Info, string title = "")
    {
        TempData["notification"] = $"Swal.fire('{title}', '{msg}', '{type.ToString().ToLower()}')";
    }
}
