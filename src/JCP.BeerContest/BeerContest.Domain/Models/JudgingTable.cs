namespace BeerContest.Domain.Models
{
    public class JudgingTable
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}