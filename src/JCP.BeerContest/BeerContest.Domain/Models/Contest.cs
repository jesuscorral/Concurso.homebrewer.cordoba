namespace BeerContest.Domain.Models
{
    public class Contest
    {
        public required string Id { get; set; }
        public required string Edition { get; set; }
        public required string Description { get; set; }
        public required string OrganizerEmail { get; set; }
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
        public ContestStatus Status { get; set; }
        public int MaxBeersPerParticipant { get; set; } = 3; // Default limit of 3 beers per participant
        public DateTime CreatedAt { get; set; }
    }

    public class ContestRule
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
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