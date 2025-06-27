using BeerContest.Application.Features.Users.Queries.GetUserById;
using BeerContest.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerContest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;
        private readonly ClaimsService _claimsService;

        public AccountController(
            IMediator mediator,
            ILogger<AccountController> logger,
            ClaimsService claimsService)
        {
            _mediator = mediator;
            _logger = logger;
            _claimsService = claimsService;
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
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            return Redirect(returnUrl);
        }

        [HttpPost("refresh-claims")]
        [Authorize]
        public async Task<IActionResult> RefreshClaims()
        {
            try
            {
                // Get the current user's email from claims
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("User email not found in claims");
                }

                // Get the user from the repository
                var userRepository = HttpContext.RequestServices.GetRequiredService<BeerContest.Domain.Repositories.IUserRepository>();
                var user = await userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"User with email {email} not found");
                }

                // Refresh the claims
                await _claimsService.RefreshUserClaimsAsync(user.Id);

                return Ok(new { message = "Claims refreshed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing claims");
                return StatusCode(500, "An error occurred while refreshing claims");
            }
        }
    }
}