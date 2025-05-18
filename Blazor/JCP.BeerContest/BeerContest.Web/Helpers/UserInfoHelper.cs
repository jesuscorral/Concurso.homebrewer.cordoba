using Microsoft.AspNetCore.Components.Authorization;

namespace BeerContest.Web.Helpers
{
    public static class UserInfoHelper
    {
        public static async Task<string> GetUserInfoAsync(AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                return userEmail;
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
                return string.Empty;
            }
        }
    }
}
