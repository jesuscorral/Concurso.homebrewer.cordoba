using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;

namespace BeerContest.Infrastructure.Repositories
{
    public class JudgeRepository : IJudgeRepository
    {
        private readonly BeerContestContext _context;
        private const string JudgesCollectionName = "judges";

        public JudgeRepository(BeerContestContext context)
        {
            _context = context;
        }

        public async Task<string> AddJudgeAsync(Judge judge)
        {
            var firestoreJudge = FirestoreJudge.FromJudge(judge);

            string id = await _context.AddDocumentAsync(JudgesCollectionName, firestoreJudge);

            return id;  
        }

    }
}