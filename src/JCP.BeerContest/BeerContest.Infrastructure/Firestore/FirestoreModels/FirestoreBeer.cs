using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreBeer
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public int CategoryId { get; set; }

        [FirestoreProperty]
        public string BeerStyle { get; set; }

        [FirestoreProperty]
        public double AlcoholContent { get; set; }

        [FirestoreProperty]
        public DateTime ElaborationDate { get; set; }

        [FirestoreProperty]
        public DateTime BottleDate { get; set; }

        [FirestoreProperty]
        public string Malts { get; set; }

        [FirestoreProperty]
        public string Hops { get; set; }

        [FirestoreProperty]
        public string Yeast { get; set; }

        [FirestoreProperty]
        public string Additives { get; set; }
        
        [FirestoreProperty]
        public string ParticipantId { get; set; }

        [FirestoreProperty]
        public string ParticipantEmail { get; set; }

        [FirestoreProperty]
        public string ContestId { get; set; }

        [FirestoreProperty]
        public string EntryInstructions { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        // Convert from domain model to Firestore model
        public static FirestoreBeer FromBeer(Beer beer)
        {
            return new FirestoreBeer
            {
                Id = beer.Id,
                CategoryId = (int)beer.Category,
                BeerStyle = beer.BeerStyle,
                AlcoholContent = beer.AlcoholContent,
                ElaborationDate = beer.ElaborationDate.ToUniversalTime(),
                BottleDate = beer.BottleDate.ToUniversalTime(),
                Malts = beer.Malts,
                Hops = beer.Hops,
                Yeast = beer.Yeast,
                Additives = beer.Additives ?? string.Empty,
                ParticipantId = beer.ParticpantId,
                ParticipantEmail = beer.ParticipantEmail,
                EntryInstructions = beer.EntryInstructions,
                ContestId = beer.ContestId,
                CreatedAt = beer.CreatedAt.ToUniversalTime()
            };
        }

        // Convert from Firestore model to domain model
        public Beer ToBeer()
        {
            return new Beer
            {
                Id = Id,
                Category = (BeerCategory)CategoryId,
                BeerStyle = BeerStyle,
                AlcoholContent = AlcoholContent,
                ElaborationDate = ElaborationDate.ToLocalTime(),
                BottleDate = BottleDate.ToLocalTime(),
                Malts = Malts,
                Hops = Hops,
                Yeast = Yeast,
                Additives = Additives,
                ParticpantId = ParticipantId,
                ParticipantEmail = ParticipantEmail,
                EntryInstructions = EntryInstructions,
                ContestId = ContestId,
                CreatedAt = CreatedAt.ToLocalTime()
            };
        }
    }
}