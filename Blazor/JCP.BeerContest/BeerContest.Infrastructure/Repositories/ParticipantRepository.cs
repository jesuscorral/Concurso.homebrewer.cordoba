using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;

namespace BeerContest.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "participants";

        public ParticipantRepository(BeerContestContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }

        public async Task<string> CreateAsync(Participant participant)
        {
            var firestoreParticipant = FirestoreParticipant.FromParticipant(participant);
            var id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreParticipant);
            return id; 
        }
    }
}
