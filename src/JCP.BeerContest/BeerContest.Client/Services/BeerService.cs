using BeerContest.Client.Services;
using BeerContest.Domain.Models;

namespace BeerContest.Client.Services;

public class BeerService
{
    private readonly ApiService _apiService;

    public BeerService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<BeerWithContestStatus>?> GetUserBeersAsync()
    {
        return await _apiService.GetAsync<IEnumerable<BeerWithContestStatus>>("api/beers");
    }

    public async Task<Beer?> GetBeerByIdAsync(string id)
    {
        return await _apiService.GetAsync<Beer>($"api/beers/{id}");
    }

    public async Task<string?> CreateBeerAsync(RegisterBeerDto command)
    {
        var response = await _apiService.PostAsync<RegisterBeerDto, dynamic>("api/beers", command);
        return response?.Id;
    }

    public async Task<bool> UpdateBeerAsync(string id, object command)
    {
        return await _apiService.PutAsync($"api/beers/{id}", command);
    }

    public async Task<bool> DeleteBeerAsync(string id)
    {
        return await _apiService.DeleteAsync($"api/beers/{id}");
    }
}

// DTO for creating beers from the client
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
