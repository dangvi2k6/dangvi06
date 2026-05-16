using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MotorcycleShopMVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? Array.Empty<string>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // ===== SESSION =====
            var userEmail = context.HttpContext.Session.GetString("UserEmail");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            // ===== CLAIMS =====
            var user = context.HttpContext.User;

            // ===== CHECK LOGIN =====
            bool sessionAuthenticated =
                !string.IsNullOrWhiteSpace(userEmail);

            bool claimsAuthenticated =
                user?.Identity != null &&
                user.Identity.IsAuthenticated;

            // Chưa đăng nhập bằng cả 2
            if (!sessionAuthenticated && !claimsAuthenticated)
            {
                var returnUrl =
                    context.HttpContext.Request.Path +
                    context.HttpContext.Request.QueryString;

                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    new { returnUrl });

                return;
            }

            // ===== CHECK ROLE =====
            if (_roles.Length > 0)
            {
                bool hasRole = false;

                // Role từ session
                if (!string.IsNullOrWhiteSpace(userRole))
                {
                    hasRole = _roles.Any(r =>
                        string.Equals(
                            r,
                            userRole,
                            StringComparison.OrdinalIgnoreCase));
                }

                // Role từ claims
                if (!hasRole && claimsAuthenticated)
                {
                    var roleClaim =
                        user.FindFirst(ClaimTypes.Role)?.Value;

                    if (!string.IsNullOrWhiteSpace(roleClaim))
                    {
                        hasRole = _roles.Any(r =>
                            string.Equals(
                                r,
                                roleClaim,
                                StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Không đúng role
                if (!hasRole)
                {
                    context.Result =
                        new RedirectToActionResult(
                            "AccessDenied",
                            "Account",
                            null);

                    return;
                }
            }
        }
    }
}