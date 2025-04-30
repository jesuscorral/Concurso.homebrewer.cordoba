using BeerContest.Application;
using BeerContest.Application.Features.Users.Commands.RegisterGoogleUser;
using BeerContest.Application.Features.Users.Queries.GetUserById;
using BeerContest.Domain.Models;
using BeerContest.Infrastructure;
using MediatR;
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
    // Use the default callback path for Google authentication
    options.CallbackPath = "/signin-google"; // Default ASP.NET Core callback path
    options.Scope.Add("email");
    options.Scope.Add("profile");
    options.SaveTokens = true;
    options.Events.OnTicketReceived = async context =>
    {
        try
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            
            // Extract claims
            var googleId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = identity.FindFirst(ClaimTypes.Name)?.Value;
            var email = identity.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                return;
            }
            
            // Get the mediator service
            var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
            
            // Register or get existing user
            var command = new RegisterGoogleUserCommand
            {
                GoogleId = googleId,
                Email = email,
                DisplayName = name,
                Role = UserRole.Participant // Default to Participant
            };
            
            var userId = await mediator.Send(command);
            
            // Get user role
            var user = await mediator.Send(new GetUserByIdQuery { Id = userId });
            if (user != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            }
        }
        catch (Exception ex)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error during Google authentication");
        }
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
builder.Services.AddControllers();

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

// Map controller endpoints
app.MapControllers();

app.Run();
