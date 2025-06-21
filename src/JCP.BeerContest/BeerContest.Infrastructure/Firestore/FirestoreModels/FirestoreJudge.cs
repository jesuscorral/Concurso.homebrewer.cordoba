using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreJudge
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Surname { get; set; }

        [FirestoreProperty]
        public string Phone { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Preferences { get; set; }

        [FirestoreProperty]
        public string BcjpId { get; set; }

        [FirestoreProperty]
        public string ContestId { get; set; }

        [FirestoreProperty]
        public string ContestName { get; set; }

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
                ContestName = judge.ContestName
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
                ContestName = ContestName
            };
        }
    }
}
