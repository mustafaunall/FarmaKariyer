using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Attributes;

public class CheckSessionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            //context.Result = new RedirectResult("/Home");

            // Kullanıcının geldiği sayfayı al (referer header'dan).
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer))
            {
                // Referer yoksa ana sayfaya yönlendir.
                context.Result = new RedirectResult("~/");
            }
            else
            {
                // Kullanıcıyı geldiği sayfaya geri yönlendir.
                context.Result = new RedirectResult(referer);
            }
        }

        base.OnActionExecuting(context);
    }
}
