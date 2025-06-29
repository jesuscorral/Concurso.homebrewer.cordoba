using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Firestore;

namespace BeerContest.Web.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection InitializeDatabase(this IServiceCollection services)
        {
             // Register enhanced Firestore context
            services.AddSingleton<IFirestoreContext, EnhancedFirestoreContext>();
            
            return services;
        }

    }
}
