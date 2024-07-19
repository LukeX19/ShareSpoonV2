using System.Security.Claims;

namespace ShareSpoon.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserIdClaimValue(this HttpContext context)
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            return claimsIdentity?.FindFirst("uid")?.Value;
        }
    }
}
