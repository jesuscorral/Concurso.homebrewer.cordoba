using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BeerContest.Web.Services
{
    public class ClaimsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ClaimsService> _logger;

        public ClaimsService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ClaimsService> logger)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Refreshes the claims for the current user
        /// </summary>
        public async Task RefreshUserClaimsAsync(string userId)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogWarning("Cannot refresh claims: HttpContext is null");
                    return;
                }

                // Get the current user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Cannot refresh claims: User with ID {userId} not found");
                    return;
                }

                // Get the current principal
                var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!authenticateResult.Succeeded)
                {
                    _logger.LogWarning("Cannot refresh claims: Authentication failed");
                    return;
                }

                var identity = authenticateResult.Principal.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    _logger.LogWarning("Cannot refresh claims: Identity is null");
                    return;
                }

                // Remove existing role claims
                var existingRoleClaims = identity.FindAll(ClaimTypes.Role).ToList();
                foreach (var claim in existingRoleClaims)
                {
                    identity.RemoveClaim(claim);
                }

                // Add updated role claims
                foreach (var role in user.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }

                // Sign in with the updated claims
                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                _logger.LogInformation($"Claims refreshed for user {userId} with roles: {string.Join(", ", user.Roles)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error refreshing claims for user {userId}");
            }
        }

        /// <summary>
        /// Creates claims for a user based on their roles
        /// </summary>
        public List<Claim> CreateUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.GoogleId),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                // Add a custom claim for the user ID
                new Claim("UserId", user.Id)
            };

            // Add role claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            return claims;
        }
    }
}