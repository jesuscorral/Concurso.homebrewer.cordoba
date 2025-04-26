using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeerContest.Domain.Models;

namespace BeerContest.Domain.Repositories
{
    public interface IContestRepository
    {
        Task<Contest> GetByIdAsync(string id);
        Task<IEnumerable<Contest>> GetAllAsync();
        Task<IEnumerable<Contest>> GetActiveContestsAsync();
        Task<string> CreateAsync(Contest contest);
        Task UpdateAsync(Contest contest);
        Task DeleteAsync(string id);
        Task AddRuleAsync(string contestId, ContestRule rule);
        Task UpdateRuleAsync(string contestId, ContestRule rule);
        Task DeleteRuleAsync(string contestId, string ruleId);
        Task AddCategoryAsync(string contestId, BeerCategory category);
        Task UpdateCategoryAsync(string contestId, BeerCategory category);
        Task DeleteCategoryAsync(string contestId, string categoryId);
        Task UpdateStatusAsync(string contestId, ContestStatus status);
    }
}