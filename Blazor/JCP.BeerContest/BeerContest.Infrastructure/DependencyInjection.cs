using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeerContest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           
            // Register Firestore context
            services.AddSingleton(new BeerContestContext(configuration));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IContestRepository, ContestRepository>();
            
            return services;
        }
    }
}