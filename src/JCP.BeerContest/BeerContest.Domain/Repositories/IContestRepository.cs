using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IContestRepository
    {
        /// <summary>
        /// Creates a new contest in the database.
        /// </summary>
        /// <param name="contest">Contest information</param>
        /// <returns></returns>
        Task<string> CreateAsync(Contest contest);

        /// <summary>
        /// Gets all contests from the database.
        /// </summary>
        /// <returns>A list of all contests</returns>
        Task<IEnumerable<Contest>> GetAllAsync();

        /// <summary>
        /// Gets a contest by its ID.
        /// </summary>
        /// <param name="id">The contest ID</param>
        /// <returns>The contest if found, null otherwise</returns>
        Task<Contest> GetByIdAsync(string id);

        /// <summary>
        /// Updates an existing contest in the database.
        /// </summary>
        /// <param name="contest">The updated contest information</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        Task UpdateAsync(Contest contest);
    }
}
