using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace BeerContest.Web.Infrastructure.Extensions
{
    public static class KeyvaultExtensions
    {
        /// <summary>
        /// Add keyvault secrets
        /// </summary>
        /// <param name="config"></param>
        /// <param name="keyVaultUrlKey"></param>
        public static void AddKeyVaultConfiguration(this ConfigurationManager config, string keyVaultUrlKey)
        {
            var keyVaultUrl = config[keyVaultUrlKey];


            if (!string.IsNullOrWhiteSpace(keyVaultUrl))
            {
                Console.WriteLine($"Using KeyVault URL: {keyVaultUrl}");
                var kvClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

                config.AddAzureKeyVault(kvClient, new KeyVaultSecretManager());
            }
        }

    }
}
