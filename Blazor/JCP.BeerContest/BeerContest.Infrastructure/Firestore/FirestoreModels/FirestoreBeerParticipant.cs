using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreBeerParticipant
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        
        [FirestoreProperty]
        public string BeerId { get; set; }
        
        [FirestoreProperty]
        public string ParticipantId { get; set; }
        
        [FirestoreProperty]
        public string EntryInstructions { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        public static FirestoreBeerParticipant FromBeerParticipant(BeerParticipant beerParticipant)
        {
            return new FirestoreBeerParticipant
            {
                Id = beerParticipant.Id,
                BeerId = beerParticipant.BeerId,
                ParticipantId = beerParticipant.ParticipantId,
                EntryInstructions = beerParticipant.EntryInstructions,
                CreatedAt = beerParticipant.CreatedAt.ToUniversalTime()
            };
        }
        public BeerParticipant ToBeerParticipant()
        {
            return new BeerParticipant
            {
                Id = Id,
                BeerId = BeerId,
                ParticipantId = ParticipantId,
                EntryInstructions = EntryInstructions,
                CreatedAt = CreatedAt.ToLocalTime()
            };
        }
    }
}
