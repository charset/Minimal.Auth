using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Minimal.Auth.MockData;
using Minimal.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddHttpClient();
services.AddSingleton<UserService>();
services.AddMinimalAuth();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
services.AddAuthorization();
services.AddBlazoredLocalStorage();
services.AddAntDesign();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapMinimalAuth();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
