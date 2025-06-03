using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;

namespace BeerContest.Infrastructure.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "contests";

        public ContestRepository(BeerContestContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }

        public async Task<string> CreateAsync(Contest contest)
        {
            var firestoreContest = FireStoreContest.FromContest(contest);
            string id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreContest);

            return id;
        }

        public async Task<IEnumerable<Contest>> GetAllAsync()
        {
            var firestoreContests = await _firestoreContext.GetCollectionAsync<FireStoreContest>(CollectionName);
            return firestoreContests.Select(fc => fc.ToContest());
        }

        public async Task<Contest> GetByIdAsync(string id)
        {
            var firestoreContest = await _firestoreContext.GetDocumentAsync<FireStoreContest>(CollectionName, id);
            return firestoreContest?.ToContest();
        }

        public async Task<bool> UpdateAsync(Contest contest)
        {
            try
            {
                var firestoreContest = FireStoreContest.FromContest(contest);
                await _firestoreContext.SetDocumentAsync(CollectionName, contest.Id, firestoreContest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
