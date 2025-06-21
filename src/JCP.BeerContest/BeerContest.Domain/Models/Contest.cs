namespace BeerContest.Domain.Models
{
    public class Contest
    {
        public string Id { get; set; }
        public string Edition { get; set; }
        public string Description { get; set; }
        public string OrganizerEmail { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime ShipphingStartDate { get; set; }
        public DateTime ShipphingEndDate { get; set; }
        public double EntryFee1Beer { get; set; }
        public double EntryFee2Beer { get; set; }
        public double EntryFee3Beer { get; set; }
        public double Discount { get; set; } = 0.0; // Default discount is 0%
        public List<ContestRule> Rules { get; set; } = new List<ContestRule>();
        public List<BeerCategory> Categories { get; set; } = new List<BeerCategory>();
        //public List<BeerStyles> Styles { get; set; } = new List<BeerStyles>();
        public ContestStatus Status { get; set; }
        public int MaxBeersPerParticipant { get; set; } = 3; // Default limit of 3 beers per participant
    }

    public class ContestRule
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public enum ContestStatus
    {
        Draft,
        RegistrationOpen,
        RegistrationClosed,
        Judging,
        Completed
    }
}