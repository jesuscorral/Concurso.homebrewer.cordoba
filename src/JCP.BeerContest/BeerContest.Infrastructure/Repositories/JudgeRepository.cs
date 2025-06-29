using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    public class JudgeRepository : FirestoreRepositoryBase<Judge, FirestoreJudge, string>, IJudgeRepository
    {
        protected override string CollectionName => "judges";

        public JudgeRepository(IFirestoreContext context, 
            ILogger<JudgeRepository> logger)
            : base(context, logger)
        {
        }

        protected override Judge ToDomainEntity(FirestoreJudge firestoreModel)
        {
            return firestoreModel.ToJudge();
        }

        protected override FirestoreJudge ToFirestoreModel(Judge entity)
        {
            return FirestoreJudge.FromJudge(entity);
        }

        protected override string KeyToString(string key) => key;

        protected override string StringToKey(string documentId) => documentId;


        public async Task<string> CreateAsync(Judge judge)
        {
            
            try
            {
                judge.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(judge);
                _logger.LogInformation("Created judge {JudgeId}", id);
                return id;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Failed to create judge: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Judge>> GetAllByContestAsync(string contestId)
        {
            try
            { 
                    var judges = await FindAsync(query => query.WhereEqualTo("ContestId", contestId));
                    _logger.LogInformation("Retrieved {Count} judges for contest {ContestId}", judges.Count(), contestId);
                    return judges;      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get judges for contest {contest}", contestId);
                throw;
            }
        }
    }
}