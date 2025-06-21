using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreParticipant
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string ACCEMemberNumber { get; set; }

        [FirestoreProperty]
        public string FullName { get; set; }

        [FirestoreProperty]
        public DateTime BirthDate { get; set; }

        [FirestoreProperty]
        public string Phone { get; set; }

        [FirestoreProperty]
        public string EmailUser { get; set; } // Email User from Users table

        public static FirestoreParticipant FromParticipant(Participant participant)
        {
            return new FirestoreParticipant
            {
                Id = participant.Id,
                ACCEMemberNumber = participant.ACCEMemberNumber,
                FullName = participant.FullName,
                BirthDate = participant.BirthDate.ToUniversalTime(),
                Phone = participant.Phone,
                EmailUser = participant.EmailUser 
            };
        }

        public Participant ToParticipant()
        {
            return new Participant
            {
                Id = Id,
                ACCEMemberNumber = ACCEMemberNumber,
                FullName = FullName,
                BirthDate = BirthDate.ToLocalTime(),
                Phone = Phone,
                EmailUser = EmailUser
            };
        }
    }
}
