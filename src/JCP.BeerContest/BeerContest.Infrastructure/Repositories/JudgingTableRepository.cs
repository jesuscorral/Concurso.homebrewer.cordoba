using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    public class JudgingTableRepository : FirestoreRepositoryBase<JudgingTable, FirestoreJudgingTable, string>, IJudgingTableRepository
    {
        protected override string CollectionName => "judging_tables";

        private readonly IBeerRepository _beerRepository;
        private readonly IJudgeRepository _judgeRepository;

        public JudgingTableRepository(
            IFirestoreContext context,
            ILogger<JudgingTableRepository> logger,
            IBeerRepository beerRepository,
            IJudgeRepository judgeRepository)
            : base(context, logger)
        {
            _judgeRepository = judgeRepository ?? throw new ArgumentNullException(nameof(judgeRepository), "Judge repository cannot be null");
            _beerRepository = beerRepository ?? throw new ArgumentNullException(nameof(beerRepository), "Beer repository cannot be null");
        }

        protected override JudgingTable ToDomainEntity(FirestoreJudgingTable firestoreModel)
        {
            return firestoreModel.ToJudgingTable();
        }

        protected override FirestoreJudgingTable ToFirestoreModel(JudgingTable entity)
        {
            return FirestoreJudgingTable.FromJudgingTable(entity);
        }

        protected override string KeyToString(string key) => key;

        protected override string StringToKey(string documentId) => documentId;


        public async Task<IEnumerable<JudgingTable>> GetByContestIdAsync(string contestId)
        {
            try
            {
                var judgingTables = await FindAsync(query => query.WhereEqualTo("ContestId", contestId));
                return judgingTables;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get judging table for contest {ContestId}", contestId);
                throw;
            }
        }

        public async Task<JudgingTable> GetByIdAsync(string id)
        {
            try
            {
                var judgingTables = await FindAsync(query => query.WhereEqualTo("Id", id));
                return judgingTables.FirstOrDefault() ?? throw new KeyNotFoundException($"Judging table with Id {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get judging table by Id {id}", id);
                throw;
            }
        }

        public async Task<string> CreateAsync(JudgingTable table)
        {
            try
            {
                table.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(table);
                _logger.LogInformation("Created judging table with Id {TableId}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create judging table");
                throw;
            }
        }

        public async Task UpdateAsync(JudgingTable table)
        {
            try
            {
                await UpdateAsync(table.Id, table);
                _logger.LogInformation("Updated table {TableId}", table.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update table {TableId}", table.Id);
                throw;
            }

        }

        public async Task DeleteAsync(string id)
        {

            try
            {

                await DeleteAsync(id);
                _logger.LogInformation("Deleted judging table with Id {TableId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete judging table with Id {TableId}", id);
            }
        }

        public async Task<IEnumerable<Beer>> GetUnassignedBeersAsync(string contestId)
        {
            // Get all beers for the contest
            var allBeers = await _beerRepository.GetByContestAsync(contestId);

            // Get all tables for the contest
            var tables = await GetByContestIdAsync(contestId);

            // Get all beer IDs that are already assigned to tables
            var assignedBeerIds = tables.SelectMany(t => t.BeerIds).ToHashSet();

            // Return beers that are not assigned to any table
            return allBeers.Where(b => !assignedBeerIds.Contains(b.Id)).ToList();
        }

        public async Task<IEnumerable<Judge>> GetUnassignedJudgesAsync(string contestId)
        {
            // Get all judges for the contest
            var allJudges = await _judgeRepository.GetAllByContestAsync(contestId);

            // Get all tables for the contest
            var tables = await GetByContestIdAsync(contestId);

            // Get all judge IDs that are already assigned to tables
            var assignedJudgeIds = tables.SelectMany(t => t.JudgeIds).ToHashSet();

            // Return judges that are not assigned to any table
            return allJudges.Where(j => !assignedJudgeIds.Contains(j.Id)).ToList();
        }

        public async Task<bool> AreAllBeersAssignedAsync(string contestId)
        {
            var unassignedBeers = await GetUnassignedBeersAsync(contestId);
            return !unassignedBeers.Any();
        }
    }
}