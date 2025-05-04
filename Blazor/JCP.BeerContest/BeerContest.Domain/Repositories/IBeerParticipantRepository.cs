using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IBeerParticipantRepository
    {
        Task<string> CreateAsync(BeerParticipant beer);
    }
}
