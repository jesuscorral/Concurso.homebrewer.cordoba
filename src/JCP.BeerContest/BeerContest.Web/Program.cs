using BeerContest.Application;
using BeerContest.Application.Features.Users.Commands.RegisterGoogleUser;
using BeerContest.Application.Features.Users.Queries.GetUserById;
using BeerContest.Domain.Models;
using BeerContest.Infrastructure;
using BeerContest.Web.Infrastructure.Extensions;
using BeerContest.Web.Services;
using Blazored.Toast;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppConfiguration();

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
    options.ClientId = builder.Configuration[Constants.Google.Authentication.CLIENT_ID] ?? throw new InvalidOperationException("Google Client ID is not configured.");
    options.ClientSecret = builder.Configuration[Constants.Google.Authentication.CLIENT_SECRET] ?? throw new InvalidOperationException("Google Client Secret is not configured.");
    
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
                Roles = [UserRole.Participant] // Default to Participant
            };
            
            var userId = await mediator.Send(command);
            
            // Get user role
            var user = await mediator.Send(new GetUserByIdQuery { Id = userId });
            if (user != null)
            {
                // Get the claims service
                var claimsService = context.HttpContext.RequestServices.GetRequiredService<ClaimsService>();
                
                // Clear existing claims
                var existingClaims = identity.Claims.ToList();
                foreach (var claim in existingClaims)
                {
                    identity.RemoveClaim(claim);
                }
                
                // Add all user claims including roles
                var claims = claimsService.CreateUserClaims(user);
                foreach (var claim in claims)
                {
                    identity.AddClaim(claim);
                }
                
                // Log the role for debugging
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation($"User authenticated with roles: {string.Join(", ", user.Roles)}");
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
    // Make sure policy role names match exactly with the enum value strings
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireRole(UserRole.Administrator.ToString()));
    
    options.AddPolicy("RequireJudgeRole", policy =>
        policy.RequireRole(UserRole.Judge.ToString(), UserRole.Administrator.ToString()));
});

// Add application services
builder.Services.AddApplication();
builder.Services.AddRepositories();
builder.Services.InitializeDatabase(builder.Configuration);
builder.Services.AddControllers();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add ClaimsService
builder.Services.AddScoped<ClaimsService>();

// Add HttpClient factory
builder.Services.AddHttpClient();

// Add health checks
builder.Services.AddHealthChecks();

builder.Services.AddBlazoredToast();

builder.Services
    .AddBlazorise()
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

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

//// Initialize Firebase service
//var firebaseService = app.Services.GetRequiredService<BeerContest.Infrastructure.Services.ISecureFirebaseService>();
//await firebaseService.InitializeAsync(builder.Configuration);

app.MapRazorComponents<BeerContest.Web.Components.App>()
    .AddInteractiveServerRenderMode();

// Map controller endpoints
app.MapControllers();

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
