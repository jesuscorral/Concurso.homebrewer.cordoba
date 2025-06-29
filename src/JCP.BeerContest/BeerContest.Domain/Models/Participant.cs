namespace BeerContest.Domain.Models
{
    public class Participant
    {
        public required string Id { get; set; }
        public required string ACCEMemberNumber { get; set; }
        public required string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Phone { get; set; }
        public required string EmailUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}