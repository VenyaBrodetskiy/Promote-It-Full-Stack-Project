using dotNetBackend.Common;

namespace dotNetBackend.Helpers
{
    public class HttpHelper
    {
        public static int GetUserTypeId(HttpContext httpContext)
        {
            return int.Parse(
                httpContext.User.Claims
                    .Where(c => c.Type == TokenClaims.UserTypeId)
                    .Select(c => c.Value).First());
        }

        public static int GetUserId(HttpContext httpContext)
        {
            return int.Parse(
                httpContext.User.Claims
                    .Where(c => c.Type == TokenClaims.UserId)
                    .Select(c => c.Value).First());
        }
    }
}
