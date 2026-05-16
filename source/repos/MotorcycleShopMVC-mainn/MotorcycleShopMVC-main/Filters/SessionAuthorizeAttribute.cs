using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MotorcycleShopMVC.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                var req = context.HttpContext.Request;
                var returnUrl = req.Path + req.QueryString; // ví dụ: /Cart/Checkout?x=1

                context.Result = new RedirectToActionResult(
                    actionName: "Login",
                    controllerName: "Account",
                    routeValues: new { returnUrl }
                );
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}