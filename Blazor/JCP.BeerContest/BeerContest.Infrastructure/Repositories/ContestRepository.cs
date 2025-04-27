using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "contests";

        public ContestRepository(BeerContestContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }

        public async Task<Contest> GetByIdAsync(string id)
        {
            return await _firestoreContext.GetDocumentAsync<Contest>(CollectionName, id);
        }

        public async Task<IEnumerable<Contest>> GetAllAsync()
        {
            return await _firestoreContext.GetCollectionAsync<Contest>(CollectionName);
        }

        public async Task<IEnumerable<Contest>> GetActiveContestsAsync()
        {
            Query query = _firestoreContext.CreateQuery(CollectionName)
                .WhereIn("Status", new object[] 
                { 
                    ContestStatus.RegistrationOpen, 
                    ContestStatus.RegistrationClosed, 
                    ContestStatus.Judging 
                });

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<Contest>())
                .ToList();
        }

        public async Task<string> CreateAsync(Contest contest)
        {
            return await _firestoreContext.AddDocumentAsync(CollectionName, contest);
        }

        public Task UpdateAsync(Contest contest)
        {
            return _firestoreContext.SetDocumentAsync(CollectionName, contest.Id, contest);
        }

        public Task DeleteAsync(string id)
        {
            return _firestoreContext.DeleteDocumentAsync(CollectionName, id);
        }

        public async Task AddRuleAsync(string contestId, ContestRule rule)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            rule.Id = Guid.NewGuid().ToString();
            
            if (contest.Rules == null)
            {
                contest.Rules = new List<ContestRule>();
            }

            contest.Rules.Add(rule);
            await UpdateAsync(contest);
        }

        public async Task UpdateRuleAsync(string contestId, ContestRule rule)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            var existingRule = contest.Rules?.FirstOrDefault(r => r.Id == rule.Id);
            if (existingRule == null)
            {
                throw new ArgumentException($"Rule with ID {rule.Id} not found");
            }

            // Update the rule
            int index = contest.Rules.IndexOf(existingRule);
            contest.Rules[index] = rule;

            await UpdateAsync(contest);
        }

        public async Task DeleteRuleAsync(string contestId, string ruleId)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            var existingRule = contest.Rules?.FirstOrDefault(r => r.Id == ruleId);
            if (existingRule == null)
            {
                throw new ArgumentException($"Rule with ID {ruleId} not found");
            }

            contest.Rules.Remove(existingRule);
            await UpdateAsync(contest);
        }

        public async Task AddCategoryAsync(string contestId, BeerCategory2 category)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            category.Id = Guid.NewGuid().ToString();
            
            if (contest.Categories == null)
            {
                contest.Categories = new List<BeerCategory2>();
            }

            contest.Categories.Add(category);
            await UpdateAsync(contest);
        }

        public async Task UpdateCategoryAsync(string contestId, BeerCategory2 category)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            var existingCategory = contest.Categories?.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {category.Id} not found");
            }

            // Update the category
            int index = contest.Categories.IndexOf(existingCategory);
            contest.Categories[index] = category;

            await UpdateAsync(contest);
        }

        public async Task DeleteCategoryAsync(string contestId, string categoryId)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            var existingCategory = contest.Categories?.FirstOrDefault(c => c.Id == categoryId);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {categoryId} not found");
            }

            contest.Categories.Remove(existingCategory);
            await UpdateAsync(contest);
        }

        public async Task UpdateStatusAsync(string contestId, ContestStatus status)
        {
            var contest = await GetByIdAsync(contestId);
            if (contest == null)
            {
                throw new ArgumentException($"Contest with ID {contestId} not found");
            }

            contest.Status = status;
            await UpdateAsync(contest);
        }
    }
}