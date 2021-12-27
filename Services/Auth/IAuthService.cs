using Minimal.Auth.MockData;
using Minimal.Auth.Models.Auth;

namespace Minimal.Auth.Services.Auth {
    public interface IAuthService {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<LogoutResponse> Logout(LogoutRequest logoutRequest);
        Task<CurrentUser> CurrentUserInfo();
    }
}
