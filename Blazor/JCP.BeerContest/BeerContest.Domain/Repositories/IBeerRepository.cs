using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IBeerRepository
    {
        Task<Beer> GetByIdAsync(string id);
        Task<IEnumerable<Beer>> GetAllAsync();
        Task<IEnumerable<Beer>> GetByContestAsync(string contestId);
        Task<IEnumerable<Beer>> GetByBrewerAsync(string brewerId);
        Task<IEnumerable<Beer>> GetAssignedToJudgeAsync(string judgeId);
        Task<int> GetBrewerBeerCountAsync(string brewerId, string contestId);
        Task<string> CreateAsync(Beer beer);
        Task UpdateAsync(Beer beer);
        Task DeleteAsync(string id);
        Task AddRatingAsync(string beerId, BeerRating rating);
        Task UpdateRatingAsync(BeerRating rating);
        Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds);
    }
}