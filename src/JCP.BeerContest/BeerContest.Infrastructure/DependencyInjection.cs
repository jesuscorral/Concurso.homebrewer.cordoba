using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BeerContest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register Unit of Work
            services.AddScoped<IUnitOfWork, FirestoreUnitOfWork>();
            
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeerRepository, BeerRepository>();
            
            // Register traditional repositories for entities not yet migrated
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IContestRepository, ContestRepository>();
            services.AddScoped<IJudgeRepository, JudgeRepository>();
            services.AddScoped<IJudgingTableRepository, JudgingTableRepository>();

            return services;
        }
    }
}