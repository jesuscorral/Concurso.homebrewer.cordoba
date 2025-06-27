using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeerContest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
           
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IContestRepository, ContestRepository>();
            services.AddScoped<IJudgeRepository, JudgeRepository>();
            services.AddScoped<IJudgingTableRepository, JudgingTableRepository>();

            return services;
        }
    }
}