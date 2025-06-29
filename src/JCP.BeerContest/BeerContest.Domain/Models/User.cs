namespace BeerContest.Domain.Models
{
    public class User
    {
        public required string Id { get; set; }
        public required string GoogleId { get; set; }
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public required List<UserRole> Roles { get; set; }
        public List<Beer> RegisteredBeers { get; set; } = new List<Beer>();
        public List<Beer> AssignedBeersForJudging { get; set; } = new List<Beer>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public enum UserRole
    {
        Participant,
        Judge,
        Administrator
    }
}