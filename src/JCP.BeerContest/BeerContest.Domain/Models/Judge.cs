namespace BeerContest.Domain.Models
{
    public class Judge
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Preferences { get; set; }
        public required string BcjpId { get; set; }
        public required string ContestId { get; set; }
        public required string ContestName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}