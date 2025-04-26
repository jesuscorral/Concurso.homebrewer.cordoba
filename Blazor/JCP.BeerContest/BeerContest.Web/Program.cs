using BeerContest.Application;
using BeerContest.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthSection["ClientId"];
    options.ClientSecret = googleAuthSection["ClientSecret"];
    options.Scope.Add("email");
    options.Scope.Add("profile");
    options.SaveTokens = true;
    options.Events.OnCreatingTicket = context =>
    {
        var identity = (ClaimsIdentity)context.Principal.Identity;
        var profileData = context.User;
        
        // Extract and map claims from Google profile
        var nameIdentifier = profileData.GetProperty("sub").GetString();
        var name = profileData.GetProperty("name").GetString();
        var email = profileData.GetProperty("email").GetString();
        
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        identity.AddClaim(new Claim(ClaimTypes.Name, name));
        identity.AddClaim(new Claim(ClaimTypes.Email, email));
        
        return Task.CompletedTask;
    };
});

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireRole("Administrator"));
    
    options.AddPolicy("RequireJudgeRole", policy =>
        policy.RequireRole("Judge", "Administrator"));
});

// Add application services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<BeerContest.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
