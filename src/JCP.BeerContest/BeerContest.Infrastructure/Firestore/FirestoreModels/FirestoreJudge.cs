using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreJudge
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public required string Surname { get; set; }

        [FirestoreProperty]
        public required string Phone { get; set; }

        [FirestoreProperty]
        public required string Email { get; set; }

        [FirestoreProperty]
        public required string Preferences { get; set; }

        [FirestoreProperty]
        public required string BcjpId { get; set; }

        [FirestoreProperty]
        public required string ContestId { get; set; }

        [FirestoreProperty]
        public required string ContestName { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        public static FirestoreJudge FromJudge(Judge judge)
        {
            return new FirestoreJudge
            {
                Id = judge.Id,
                Name = judge.Name,
                Surname = judge.Surname,
                Phone = judge.Phone,
                Email = judge.Email,
                Preferences = judge.Preferences,
                BcjpId = judge.BcjpId,
                ContestId = judge.ContestId,
                ContestName = judge.ContestName,
                CreatedAt = judge.CreatedAt.ToUniversalTime()
            };
        }

        public Judge ToJudge()
        {
            return new Judge
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                Email = Email,
                Preferences = Preferences,
                BcjpId = BcjpId,
                ContestId = ContestId,
                ContestName = ContestName,
                CreatedAt = CreatedAt.ToLocalTime()
            };
        }
    }
}
