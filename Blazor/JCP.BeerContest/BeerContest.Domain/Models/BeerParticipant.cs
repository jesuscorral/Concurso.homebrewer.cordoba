
namespace BeerContest.Domain.Models
{
    public class BeerParticipant
    {
        public string Id { get; set; }
        public string BeerId { get; set; }
        public string ParticipantId { get; set; }
        public string EntryInstructions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
