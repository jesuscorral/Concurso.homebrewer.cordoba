using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IParticipantRepository
    {
        Task<string> CreateAsync(Participant participant);
    }
}
