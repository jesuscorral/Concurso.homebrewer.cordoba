namespace BeerContest.Domain.Models
{
    public class Beer
    {
        public string Id { get; set; }
        public BeerCategory Category { get; set; }
        public string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public string Malts { get; set; }
        public string Hops { get; set; }
        public string Yeast { get; set; }
        public string Additives { get; set; }
        public string ParticpantId { get; set; }
        public string ParticipantEmail { get; set; }
        public string EntryInstructions { get; set; }
        public string ContestId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}