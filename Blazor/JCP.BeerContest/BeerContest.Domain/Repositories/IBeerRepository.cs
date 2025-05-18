using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IBeerRepository
    {
        /// <summary>
        /// Creates a new beer in the database.
        /// </summary>
        /// <param name="beer">Beer information</param>
        /// <returns></returns>
        Task<string> CreateAsync(Beer beer);

        /// <summary>
        /// Get a list of beers by participant email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IEnumerable<Beer>> GetByParticipantAsync(string email);

        Task<Beer> GetByIdAsync(string id);
        Task<IEnumerable<Beer>> GetAllAsync();
        Task<IEnumerable<Beer>> GetByContestAsync(string contestId);

        Task<int> GetBrewerBeerCountAsync(string brewerId, string contestId);


        //Task UpdateAsync(Beer beer);
        Task DeleteAsync(string id);
        Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds);
    }
}