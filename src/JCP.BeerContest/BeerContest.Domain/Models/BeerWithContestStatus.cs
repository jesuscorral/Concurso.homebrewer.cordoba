namespace BeerContest.Domain.Models
{
    public class BeerWithContestStatus : Beer
    {
        public ContestStatus ContestStatus { get; set; }
    }
}
