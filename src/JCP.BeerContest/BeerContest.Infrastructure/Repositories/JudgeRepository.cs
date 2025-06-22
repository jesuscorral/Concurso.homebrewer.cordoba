using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class JudgeRepository : IJudgeRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "judges";

        public JudgeRepository(BeerContestContext context)
        {
            _firestoreContext = context;
        }

        public async Task<string> AddJudgeAsync(Judge judge)
        {
            var firestoreJudge = FirestoreJudge.FromJudge(judge);

            string id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreJudge);

            return id;  
        }

        public async Task<IEnumerable<Judge>> GetAllByContestAsync(string contestId)
        {
            Query query = await _firestoreContext.CreateQueryAsync(CollectionName);

            query = query.WhereEqualTo("ContestId", contestId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreJudge>().ToJudge())
                .ToList();
        }
    }
}