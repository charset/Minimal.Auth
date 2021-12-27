using Microsoft.AspNetCore.Components.Authorization;
using Minimal.Auth.Services.Auth;

namespace Minimal.Auth.Services {
    public static class ServiceExtension {
        public static IServiceCollection AddMinimalAuth(this IServiceCollection services) {
            services.AddScoped<CustomStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(implementationFactory => implementationFactory.GetRequiredService<CustomStateProvider>());
            services.AddScoped<IAuthService, AuthServices>();
            return services;
        }
    }
}
