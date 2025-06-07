namespace BeerContest.Domain.Models
{
    public class Judge
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Preferences { get; set; }
        public string BcjpId { get; set; }
        public string ContestId { get; set; }
        public string ContestName { get; set; }
    }
}