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
    public void Notification(string msg, NotificationType type = NotificationType.Info, string title = "", string okText = "Tamam")
    {
        TempData["notification"] = $"Swal.fire({{title: '{title}', text: '{msg}', icon: '{type.ToString().ToLower()}', confirmButtonText: '{okText}'}})";
    }
}
