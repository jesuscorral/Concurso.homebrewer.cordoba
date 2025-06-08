using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IJudgeRepository
    {
        Task<string> AddJudgeAsync(Judge judge);
        Task<IEnumerable<Judge>> GetAllByContestAsync(string ContestId);
    }
}