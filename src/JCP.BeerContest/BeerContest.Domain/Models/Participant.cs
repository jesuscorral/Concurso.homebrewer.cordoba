namespace BeerContest.Domain.Models
{
    public class Participant
    {
        public string Id { get; set; }
        public string ACCEMemberNumber { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string EmailUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}