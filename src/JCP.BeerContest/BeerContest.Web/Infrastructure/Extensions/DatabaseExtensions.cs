using BeerContest.Infrastructure.Firestore;

namespace BeerContest.Web.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection InitializeDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Firestore context
            services.AddSingleton(new BeerContestContext(configuration));

            return services;
        }

    }
}
