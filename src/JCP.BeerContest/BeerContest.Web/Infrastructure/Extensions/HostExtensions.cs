using BeerContest.Infrastructure;

namespace BeerContest.Web.Infrastructure.Extensions
{
    public static class HostExtensions
    {
        public static void ConfigureAppConfiguration(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;

            if (environment.IsDevelopment())
            {
                // Load user secrets (managed by dotnet user-secrets command)
                configuration.AddUserSecrets<Program>();
                
            }
            else
            {
                configuration.AddKeyVaultConfiguration(Constants.KeyVault.ConfigurationKeys.URL);
            }
        }
    }
}
