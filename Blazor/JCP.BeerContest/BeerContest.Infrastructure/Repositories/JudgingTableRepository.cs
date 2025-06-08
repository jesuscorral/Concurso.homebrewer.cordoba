using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class JudgingTableRepository : IJudgingTableRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string TableCollectionName = "judging_tables";
        private const string BeerCollectionName = "beers";
        private const string JudgeCollectionName = "judges";
        private readonly IBeerRepository _beerRepository;
        private readonly IJudgeRepository _judgeRepository;

        public JudgingTableRepository(
            BeerContestContext firestoreContext, 
            IBeerRepository beerRepository,
            IJudgeRepository judgeRepository)
        {
            _firestoreContext = firestoreContext;
            _beerRepository = beerRepository;
            _judgeRepository = judgeRepository;
        }

        public async Task<IEnumerable<JudgingTable>> GetAllAsync(string contestId)
        {
            Query query = _firestoreContext.CreateQuery(TableCollectionName)
                .WhereEqualTo("ContestId", contestId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreJudgingTable>().ToJudgingTable())
                .ToList();
        }

        public async Task<JudgingTable> GetByIdAsync(string id)
        {
            var firestoreTable = await _firestoreContext.GetDocumentAsync<FirestoreJudgingTable>(TableCollectionName, id);
            return firestoreTable?.ToJudgingTable();
        }

        public async Task<string> CreateAsync(JudgingTable table)
        {
            if (string.IsNullOrEmpty(table.Id))
            {
                table.Id = Guid.NewGuid().ToString();
            }

            table.CreatedAt = DateTime.Now;
            
            var firestoreTable = FirestoreJudgingTable.FromJudgingTable(table);
            await _firestoreContext.SetDocumentAsync(TableCollectionName, table.Id, firestoreTable);
            
            return table.Id;
        }

        public async Task UpdateAsync(JudgingTable table)
        {
            table.UpdatedAt = DateTime.Now;
            
            var firestoreTable = FirestoreJudgingTable.FromJudgingTable(table);
            await _firestoreContext.SetDocumentAsync(TableCollectionName, table.Id, firestoreTable);
        }

        public async Task DeleteAsync(string id)
        {
            await _firestoreContext.DeleteDocumentAsync(TableCollectionName, id);
        }

        public async Task<IEnumerable<Beer>> GetUnassignedBeersAsync(string contestId)
        {
            // Get all beers for the contest
            var allBeers = await _beerRepository.GetByContestAsync(contestId);
            
            // Get all tables for the contest
            var tables = await GetAllAsync(contestId);
            
            // Get all beer IDs that are already assigned to tables
            var assignedBeerIds = tables.SelectMany(t => t.BeerIds).ToHashSet();
            
            // Return beers that are not assigned to any table
            return allBeers.Where(b => !assignedBeerIds.Contains(b.Id)).ToList();
        }

        public async Task<IEnumerable<Judge>> GetUnassignedJudgesAsync(string contestId)
        {
            // Get all judges for the contest
            Query judgeQuery = _firestoreContext.CreateQuery(JudgeCollectionName)
                .WhereEqualTo("ContestId", contestId);

            QuerySnapshot judgeSnapshot = await judgeQuery.GetSnapshotAsync();
            var allJudges = judgeSnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreJudge>().ToJudge())
                .ToList();
            
            // Get all tables for the contest
            var tables = await GetAllAsync(contestId);
            
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