using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IBeerRepository
    {
        Task<Beer> GetByIdAsync(string id);
        Task<IEnumerable<Beer>> GetAllAsync();
        Task<IEnumerable<Beer>> GetByContestAsync(string contestId);
        Task<IEnumerable<Beer>> GetByBrewerAsync(string brewerId);
        Task<int> GetBrewerBeerCountAsync(string brewerId, string contestId);
        Task<string> CreateAsync(Beer beer);
        //Task UpdateAsync(Beer beer);
        Task DeleteAsync(string id);
        Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds);
    }
}