using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IParticipantRepository
    {
        /// <summary>
        /// Create a new participant
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        Task<string> CreateAsync(Participant participant);

        /// <summary>
        /// Get a participant by their email user
        /// </summary>
        /// <param name="emailUser"></param>
        /// <returns></returns>
        Task<Participant> GetByEmailUserAsync(string emailUser);
    }
}
