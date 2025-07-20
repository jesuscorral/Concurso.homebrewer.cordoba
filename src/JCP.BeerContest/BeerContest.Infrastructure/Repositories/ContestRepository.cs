using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    public class ContestRepository : FirestoreRepositoryBase<Contest, FireStoreContest, string>, IContestRepository
    {
        protected override string CollectionName => "contests";

        public ContestRepository(IFirestoreContext context,
        ILogger<ContestRepository> logger) 
            : base(context, logger)
        {
        }

        protected override Contest ToDomainEntity(FireStoreContest firestoreModel)
        {
            return firestoreModel.ToContest();
        }

        protected override FireStoreContest ToFirestoreModel(Contest entity)
        {
            return FireStoreContest.FromContest(entity);
        }

        protected override string StringToKey(string key)
        {
            return key;
        }

        protected override string KeyToString(string documentId)
        {
            return documentId;
        }

        public async Task<string> CreateAsync(Contest contest)
        {
            try
            {
                contest.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(contest);
                _logger.LogInformation("Created contest {ContestId}", id);
                return id;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Failed to create contest: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Contest>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Contest> GetByIdAsync(string id)
        {
            var contest = await base.GetByIdAsync(id);
            return contest ?? throw new InvalidOperationException($"Contest with Id {id} not found");
        }

        public async Task UpdateAsync(Contest contest)
        {
            try
            {
                await UpdateAsync(contest.Id, contest);
                _logger.LogInformation("Updated contest {ContestId}", contest.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update beer {BeerId}", contest.Id);
                throw;
            }
        }
    }
}
