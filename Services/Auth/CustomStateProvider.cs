using Microsoft.AspNetCore.Components.Authorization;
using Minimal.Auth.Models.Auth;
using System.Security.Claims;

namespace Minimal.Auth.Services.Auth {
    public class CustomStateProvider : AuthenticationStateProvider {
        private readonly IAuthService api;

        public CustomStateProvider(IAuthService api) { this.api = api; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var identity = new ClaimsIdentity();
            var currentUser = await GetCurrentUser();
            if (currentUser.IsAuthenticated) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, currentUser.Claims[ClaimTypes.Name])
                };
                for (int i = 0; i < currentUser.Roles.Count; i++) {
                    claims.Add(new Claim(ClaimTypes.Role, currentUser.Roles[i]));
                }
                identity = new ClaimsIdentity(claims, "Password");
            }

            var state = new AuthenticationState(new ClaimsPrincipal(identity));
            return state;
        }

        private async Task<CurrentUser> GetCurrentUser() {
            return await api.CurrentUserInfo();
        }

        public async Task<LogoutResponse> Logout(LogoutRequest logoutRequest) {
            var response = await api.Logout(logoutRequest);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return response;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) {
            var response = await api.Login(loginRequest);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return response;
        }

        public void Refresh(string token) {
            if (string.IsNullOrEmpty(token)) return;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
