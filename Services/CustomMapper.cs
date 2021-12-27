using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Minimal.Auth.MockData;
using Minimal.Auth.Models.Auth;
using System.Security.Claims;

namespace Minimal.Auth.Services {
    public static class CustomMapper {
        /// <summary>
        /// Minimal API does not support [FromForm]
        /// </summary>
        /// <param name="webApplication"></param>
        /// <remarks>https://minimal-apis.github.io/hello-minimal/#parameter-binding</remarks>
        /// <returns></returns>
        public static WebApplication MapMinimalAuth(this WebApplication webApplication) {
            webApplication.MapGet("/Auth/Current/{token}", async (string token, [FromServices]UserService service, HttpContext context) => {
                var current = new CurrentUser() {
                    IsAuthenticated = context.User.Identity != null && context.User.Identity.IsAuthenticated,
                    Claims = context.User.Claims.ToDictionary(key => key.Type, value => value.Value),
                    Roles = new List<string>()
                };
                var user = service.ValidateToken(token);
                if (user != null) {
                    current.IsAuthenticated = true;
                    current.Roles = service.GetUserRoles(user.UserId);
                    var info = service.GetUserInfo(user.UserId);
                    if (info != null) {
                        current.Claims.Add(ClaimTypes.Name,info.UserName);
                        current.Claims.Add(ClaimTypes.Actor, info.Title);
                    }
                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetClaims(current));
                } else {
                    current.IsExpired = true;
                }
                return current;
            });

            webApplication.MapPost("/auth/login", (LoginRequest request, UserService service) => service.Login(request));

            webApplication.MapPost("/Auth/Logout", (LogoutRequest request, [FromServices] UserService service) =>service.Logout(request));

            return webApplication;
        }



        private static ClaimsPrincipal GetClaims(CurrentUser user) {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Role, string.Join(',', user.Roles))
            };
            var identity = new ClaimsIdentity(claims, "Basic");
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
