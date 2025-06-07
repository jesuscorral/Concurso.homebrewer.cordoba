using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeerContest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContests(this IServiceCollection services, IConfiguration configuration)
        {

            // Register Firestore context
            services.AddSingleton(new BeerContestContext(configuration));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
           
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IContestRepository, ContestRepository>();
            services.AddScoped<IJudgeRepository, JudgeRepository>();

            return services;
        }
    }
}