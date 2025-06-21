using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FireStoreContest
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Edition { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public string OrganizerEmail { get; set; }

        [FirestoreProperty]
        public DateTime RegistrationStartDate { get; set; }

        [FirestoreProperty]
        public DateTime RegistrationEndDate { get; set; }

        [FirestoreProperty]
        public DateTime ShipphingStartDate { get; set; }

        [FirestoreProperty]
        public DateTime ShipphingEndDate { get; set; }

        [FirestoreProperty]
        public double EntryFee1Beer { get; set; }

        [FirestoreProperty]
        public double EntryFee2Beer { get; set; }

        [FirestoreProperty]
        public double EntryFee3Beer { get; set; }

        [FirestoreProperty]
        public double Discount { get; set; } = 0.0; // Default discount is 0%

        [FirestoreProperty]
        public List<ContestRule> Rules { get; set; } = new List<ContestRule>();

        [FirestoreProperty]
        public List<BeerCategory> Categories { get; set; } = new List<BeerCategory>();
        //public List<BeerStyles> Styles { get; set; } = new List<BeerStyles>();

        [FirestoreProperty]
        public ContestStatus Status { get; set; }

        [FirestoreProperty]
        public int MaxBeersPerParticipant { get; set; } = 3; // Default limit of 3 beers per participant


        public static FireStoreContest FromContest(Contest contest)
        {
            return new FireStoreContest
            {
                Id = contest.Id,
                Edition = contest.Edition,
                Description = contest.Description,
                OrganizerEmail = contest.OrganizerEmail,
                RegistrationStartDate = contest.RegistrationStartDate.ToUniversalTime(),
                RegistrationEndDate = contest.RegistrationEndDate.ToUniversalTime(),
                ShipphingStartDate = contest.ShipphingStartDate.ToUniversalTime(),
                ShipphingEndDate = contest.ShipphingEndDate.ToUniversalTime(),
                EntryFee1Beer = contest.EntryFee1Beer,
                EntryFee2Beer = contest.EntryFee2Beer,
                EntryFee3Beer = contest.EntryFee3Beer,
                Discount = contest.Discount,
                Rules = contest.Rules,
                Categories = contest.Categories,
                Status = contest.Status,
                MaxBeersPerParticipant = contest.MaxBeersPerParticipant
            };
        }

        public Contest ToContest()
        {
            return new Contest
            {
                Id = this.Id,
                Edition = this.Edition,
                Description = this.Description,
                OrganizerEmail = this.OrganizerEmail,
                RegistrationStartDate = this.RegistrationStartDate.ToLocalTime(),
                RegistrationEndDate = this.RegistrationEndDate.ToLocalTime(),
                ShipphingStartDate = this.ShipphingStartDate.ToLocalTime(),
                ShipphingEndDate = this.ShipphingEndDate.ToLocalTime(),
                EntryFee1Beer = this.EntryFee1Beer,
                EntryFee2Beer = this.EntryFee2Beer,
                EntryFee3Beer = this.EntryFee3Beer,
                Discount = this.Discount,
                Rules = this.Rules,
                Categories = this.Categories,
                Status = this.Status,
                MaxBeersPerParticipant = this.MaxBeersPerParticipant
            };
        }

    }
}