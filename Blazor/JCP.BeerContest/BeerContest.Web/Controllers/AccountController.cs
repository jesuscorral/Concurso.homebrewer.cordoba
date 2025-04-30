using BeerContest.Application.Features.Users.Commands.RegisterGoogleUser;
using BeerContest.Application.Features.Users.Queries.GetUserById;
using BeerContest.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeerContest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = "/")
        {
            // Ensure we have a valid return URL and log it
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            _logger.LogInformation($"Login initiated with returnUrl: {returnUrl}");
            
            var properties = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                AllowRefresh = true,
                IsPersistent = true
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("register")]
        public IActionResult Register(string? returnUrl = "/")
        {
            // Ensure we have a valid return URL and log it
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            _logger.LogInformation($"Registration initiated with returnUrl: {returnUrl}");
            
            var properties = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                AllowRefresh = true,
                IsPersistent = true
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // The Google callback is now handled by ASP.NET Core's authentication middleware

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string? returnUrl = "/")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl);
        }
    }
}