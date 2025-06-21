using BeerContest.Application;
using BeerContest.Application.Features.Users.Commands.RegisterGoogleUser;
using BeerContest.Application.Features.Users.Queries.GetUserById;
using BeerContest.Domain.Models;
using BeerContest.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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
    options.ClientId = googleAuthSection["ClientId"] ?? throw new InvalidOperationException("Google ClientId not configured");
    options.ClientSecret = googleAuthSection["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret not configured");
    options.CallbackPath = "/signin-google";
    options.Scope.Add("email");
    options.Scope.Add("profile");
    options.SaveTokens = true;
});

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireRole(UserRole.Administrator.ToString()));
    
    options.AddPolicy("RequireJudgeRole", policy =>
        policy.RequireRole(UserRole.Judge.ToString(), UserRole.Administrator.ToString()));
});

// Add application services
builder.Services.AddApplication();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddContests(builder.Configuration);

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
