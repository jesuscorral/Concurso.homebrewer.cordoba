using BeerContest.Infrastructure;

namespace BeerContest.Web.Infrastructure.Extensions
{
    public static class HostExtensions
    {
        public static void ConfigureAppConfiguration(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;

            configuration.AddKeyVaultConfiguration(Constants.KeyVault.ConfigurationKeys.URL);
        }
    }
}
