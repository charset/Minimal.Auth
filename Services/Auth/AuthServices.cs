using Microsoft.AspNetCore.Components;
using Minimal.Auth.Models.Auth;

namespace Minimal.Auth.Services.Auth {
    public class AuthServices : IAuthService {
        private const string TOKEN = "Token"; 
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        private readonly Blazored.LocalStorage.ILocalStorageService storage;
        private readonly IConfiguration configuration;
        private readonly string currentUserUrl, loginUrl, logoutUrl;

        public AuthServices(Blazored.LocalStorage.ILocalStorageService storage, NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration) {
            this.storage = storage;
            this.navigationManager = navigationManager;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(navigationManager.BaseUri);
            this.configuration = configuration;
            currentUserUrl = configuration["AuthUrl:Current"] ?? "Auth/Current/";
            loginUrl = configuration["AuthUrl:Login"] ?? "Auth/Login/";
            logoutUrl = configuration["AuthUrl:Logout"] ?? "Auth/Logout/";
        }

        public async Task<CurrentUser> CurrentUserInfo() {
            var user = new CurrentUser() { IsAuthenticated = false };
            string token;
            try {
                token = await storage.GetItemAsStringAsync(TOKEN);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return user;
            }

            if (!string.IsNullOrEmpty(token)) {
                try {
                    var current = await httpClient.GetFromJsonAsync<CurrentUser>($"{currentUserUrl}{token}");
                    if (current != null) {
                        if (current.IsExpired) {
                            await storage.RemoveItemAsync(TOKEN);
                        }else {
                            user = current;
                        }
                    }
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return user;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) {
            var result = await httpClient.PostAsJsonAsync(loginUrl, loginRequest);
            if (result.IsSuccessStatusCode) {
                var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
                if (response != null && response.IsSuccess) {
                    await storage.SetItemAsStringAsync(TOKEN, response.Token);
                    return response;
                }
            }
            return new LoginResponse() { IsSuccess = false };
        }

        public async Task<LogoutResponse> Logout(LogoutRequest logoutRequest) {
            var result = await httpClient.PostAsJsonAsync(logoutUrl, logoutRequest);
            if (result.IsSuccessStatusCode) {
                var response = await result.Content.ReadFromJsonAsync<LogoutResponse>();
                if (response != null && response.IsSuccess) {
                    await storage.RemoveItemAsync(TOKEN);
                    return response;
                }
            }
            return new LogoutResponse() { IsSuccess = false };
        }
    }
}
