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

        Task UpdateAsync(Beer beer);

        Task<Beer> GetByIdAsync(string id);

        /// <summary>
        /// Get the count of beers registered by a brewer for a specific contest.
        /// </summary>
        /// <param name="participantEmail"></param>
        /// <returns></returns>
        Task<int> GetBrewerBeerCountAsync(string participantEmail, string contestId);




        Task<IEnumerable<Beer>> GetAllAsync();
        Task<IEnumerable<Beer>> GetByContestAsync(string contestId);
        Task DeleteAsync(string id);
        Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds);
    }
}