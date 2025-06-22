using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;

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

        public async Task<Participant?> GetByEmailUserAsync(string emailUser)
        {
            var collection = await _firestoreContext.GetCollectionAsync<FirestoreParticipant>(CollectionName);

            var query = collection.AsQueryable()
                .Where(fp => fp.EmailUser == emailUser);

            var firestoreParticipant = query.FirstOrDefault();
            return firestoreParticipant?.ToParticipant();
        }
    }
}
