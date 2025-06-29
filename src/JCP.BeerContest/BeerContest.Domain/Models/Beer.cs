namespace BeerContest.Domain.Models
{
    public class Beer
    {
        public required string Id { get; set; }
        public BeerCategory Category { get; set; }
        public required string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public required string Malts { get; set; }
        public required string Hops { get; set; }
        public required string Yeast { get; set; }
        public required string Additives { get; set; }
        public required string ParticpantId { get; set; }
        public required string ParticipantEmail { get; set; }
        public required string EntryInstructions { get; set; }
        public required string ContestId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}