using BeerContest.Domain.Models;

namespace BeerContest.Api.Models;

public class RegisterBeerDto
{
    public BeerCategory Category { get; set; }
    public string BeerStyle { get; set; } = string.Empty;
    public double AlcoholContent { get; set; }
    public DateTime ElaborationDate { get; set; }
    public DateTime BottleDate { get; set; }
    public string Malts { get; set; } = string.Empty;
    public string Hops { get; set; } = string.Empty;
    public string Yeast { get; set; } = string.Empty;
    public string Additives { get; set; } = string.Empty;
    public string? ContestId { get; set; }
}
