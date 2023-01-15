using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using TwitterAccessor.Common;
using TwitterAccessor.Models;

namespace TwitterAccessor
{
    public class LoginAsSystemUserMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginAsSystemUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, HttpClient httpClient)
        {
            var systemCredentials = new AuthCredentials()
            {
                Login = "system",
                Password = "password"
            };

            var responseMessage = await httpClient.PostAsJsonAsync(Const.ApiLogin, systemCredentials);

            if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                //httpContext.Response.ContentType = "text/html";
                //await httpContext.Response.WriteAsync("Missing Authorization header");
                return;
            }
            else
            {
                TokenModel? token = await responseMessage.Content.ReadFromJsonAsync<TokenModel>();
                httpContext.Request.Headers.Authorization = token.Token;
                await _next.Invoke(httpContext);
            }
        }
    }

    //public static class AuthAsSystemUserExtensions
    //{
    //    public static IApplicationBuilder AuthAsSystemUser(
    //        this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<LoginAsSystemUserMiddleware>();
    //    }
    //}
}
