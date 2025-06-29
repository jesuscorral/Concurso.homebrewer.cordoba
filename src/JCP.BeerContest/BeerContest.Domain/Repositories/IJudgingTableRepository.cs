using BeerContest.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeerContest.Domain.Repositories
{
    public interface IJudgingTableRepository
    {
        Task<IEnumerable<JudgingTable>> GetByContestIdAsync(string contestId);
        Task<JudgingTable> GetByIdAsync(string id);
        Task<string> CreateAsync(JudgingTable table);
        Task UpdateAsync(JudgingTable table);
        Task DeleteAsync(string id);
        Task<IEnumerable<Beer>> GetUnassignedBeersAsync(string contestId);
        Task<IEnumerable<Judge>> GetUnassignedJudgesAsync(string contestId);
        Task<bool> AreAllBeersAssignedAsync(string contestId);
    }
}