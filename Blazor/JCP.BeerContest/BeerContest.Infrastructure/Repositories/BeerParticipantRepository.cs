using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;

namespace BeerContest.Infrastructure.Repositories
{
    public class BeerParticipantRepository : IBeerParticipantRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "beerParticipants";

        public BeerParticipantRepository(BeerContestContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }
        public async Task<string> CreateAsync(BeerParticipant beerParticipant)
        {
            var firestoreBeerParticipant = FirestoreBeerParticipant.FromBeerParticipant(beerParticipant);
            var id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreBeerParticipant);
            return id;
        }
    }
}
