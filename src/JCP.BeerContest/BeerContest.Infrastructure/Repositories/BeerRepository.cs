using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    /// <summary>
    /// Enhanced Beer repository using the new base repository pattern
    /// </summary>
    public class BeerRepository : FirestoreRepositoryBase<Beer, FirestoreBeer, string>, IBeerRepository
    {
        private readonly IContestRepository _contestRepository;
        protected override string CollectionName => "beers";

        public BeerRepository(
            IFirestoreContext context, 
            ILogger<BeerRepository> logger,
            IContestRepository contestRepository) 
            : base(context, logger)
        {
            _contestRepository = contestRepository;
        }

        protected override Beer ToDomainEntity(FirestoreBeer firestoreModel)
        {
            return firestoreModel.ToBeer();
        }

        protected override FirestoreBeer ToFirestoreModel(Beer entity)
        {
            return FirestoreBeer.FromBeer(entity);
        }

        protected override string KeyToString(string key) => key;

        protected override string StringToKey(string documentId) => documentId;

        // IBeerRepository specific methods
        public async Task<string> CreateAsync(Beer beer)
        {
            try
            {
                beer.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(beer);
                _logger.LogInformation("Created beer {BeerId} for participant {ParticipantEmail}", id, beer.ParticipantEmail);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create beer for participant {ParticipantEmail}", beer.ParticipantEmail);
                throw;
            }
        }

        public async Task<IEnumerable<BeerWithContestStatus>> GetByParticipantAsync(string email)
        {
            try
            {
                var beers = await FindAsync(query => query.WhereEqualTo("ParticipantEmail", email));
                
                var beerWithStatusList = new List<BeerWithContestStatus>();
                foreach (var beer in beers)
                {
                    var contest = await _contestRepository.GetByIdAsync(beer.ContestId);
                    var beerWithStatus = CreateBeerWithContestStatus(beer, contest);
                    beerWithStatusList.Add(beerWithStatus);
                }

                _logger.LogDebug("Retrieved {Count} beers for participant {Email}", beerWithStatusList.Count, email);
                return beerWithStatusList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beers for participant {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<BeerWithContestStatus>> GetByParticipantAndContestIdAsync(string participantEmail, string contestId)
        {
            try
            {
                var beers = await FindAsync(query => 
                    query.WhereEqualTo("ParticipantEmail", participantEmail)
                         .WhereEqualTo("ContestId", contestId));

                var contest = await _contestRepository.GetByIdAsync(contestId);
                var status = DetermineContestStatus(contest);

                var beerWithStatusList = beers.Select(beer => CreateBeerWithContestStatus(beer, contest)).ToList();

                _logger.LogDebug("Retrieved {Count} beers for participant {Email} in contest {ContestId}", 
                    beerWithStatusList.Count, participantEmail, contestId);
                return beerWithStatusList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beers for participant {Email} in contest {ContestId}", 
                    participantEmail, contestId);
                throw;
            }
        }

        public async Task UpdateAsync(Beer beer)
        {
            try
            {
                await UpdateAsync(beer.Id, beer);
                _logger.LogInformation("Updated beer {BeerId}", beer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update beer {BeerId}", beer.Id);
                throw;
            }
        }

        public async Task<IEnumerable<Beer>> GetByContestAsync(string contestId)
        {
            try
            {
                var beers = await FindAsync(query => query.WhereEqualTo("ContestId", contestId));
                _logger.LogDebug("Retrieved {Count} beers for contest {ContestId}", beers.Count(), contestId);
                return beers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beers for contest {ContestId}", contestId);
                throw;
            }
        }

        public async Task<IEnumerable<Beer>> GetByCategoryAsync(BeerCategory category)
        {
            try
            {
                var beers = await FindAsync(query => query.WhereEqualTo("CategoryId", (int)category));
                _logger.LogDebug("Retrieved {Count} beers for category {Category}", beers.Count(), category);
                return beers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beers for category {Category}", category);
                throw;
            }
        }

        public async Task<bool> ParticipantHasBeersInContestAsync(string participantEmail, string contestId)
        {
            try
            {
                var beers = await FindAsync(query => 
                    query.WhereEqualTo("ParticipantEmail", participantEmail)
                         .WhereEqualTo("ContestId", contestId)
                         .Limit(1));

                var hasBeers = beers.Any();
                _logger.LogDebug("Participant {Email} has beers in contest {ContestId}: {HasBeers}", 
                    participantEmail, contestId, hasBeers);
                return hasBeers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if participant {Email} has beers in contest {ContestId}", 
                    participantEmail, contestId);
                throw;
            }
        }

        public async Task<int> GetBeerCountByContestAsync(string contestId)
        {
            try
            {
                var count = await CountAsync(query => query.WhereEqualTo("ContestId", contestId));
                _logger.LogDebug("Contest {ContestId} has {Count} beers", contestId, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beer count for contest {ContestId}", contestId);
                throw;
            }
        }

        public async Task<(IEnumerable<Beer> Beers, int TotalCount)> GetPagedByContestAsync(
            string contestId, 
            int pageNumber, 
            int pageSize)
        {
            try
            {
                var result = await GetPagedAsync(
                    pageNumber, 
                    pageSize, 
                    query => query.WhereEqualTo("ContestId", contestId));

                _logger.LogDebug("Retrieved page {PageNumber} of beers for contest {ContestId} ({Count} of {Total})", 
                    pageNumber, contestId, result.Items.Count(), result.TotalCount);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get paged beers for contest {ContestId}", contestId);
                throw;
            }
        }

        // Additional IBeerRepository methods
        public async Task<int> GetBrewerBeerCountAsync(string participantEmail, string contestId)
        {
            try
            {
                var beers = await FindAsync(query => 
                    query.WhereEqualTo("ParticipantEmail", participantEmail)
                         .WhereEqualTo("ContestId", contestId));
                
                var count = beers.Count();
                _logger.LogDebug("Found {Count} beers for participant {Email} in contest {ContestId}", count, participantEmail, contestId);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beer count for participant {Email} in contest {ContestId}", participantEmail, contestId);
                throw;
            }
        }

        public async Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds)
        {
            try
            {
                var updateTasks = beerIds.Select(async beerId =>
                {
                    var updates = new Dictionary<string, object>
                    {
                        ["AssignedJudgeId"] = judgeId,
                        ["AssignedAt"] = DateTime.UtcNow
                    };
                    await UpdateFieldsAsync(beerId, updates);
                });

                await Task.WhenAll(updateTasks);
                
                _logger.LogInformation("Assigned {Count} beers to judge {JudgeId}", beerIds.Count(), judgeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign beers to judge {JudgeId}", judgeId);
                throw;
            }
        }

        // Explicit implementations for IBeerRepository (delegating to base class)
        async Task<Beer> IBeerRepository.GetByIdAsync(string id)
        {
            var beer = await GetByIdAsync(id);
            return beer ?? throw new InvalidOperationException($"Beer with ID {id} not found");
        }

        async Task<IEnumerable<Beer>> IBeerRepository.GetAllAsync()
        {
            return await GetAllAsync();
        }

        async Task IBeerRepository.DeleteAsync(string id)
        {
            await DeleteAsync(id);
        }

        private static BeerWithContestStatus CreateBeerWithContestStatus(Beer beer, Contest? contest)
        {
            return new BeerWithContestStatus
            {
                Id = beer.Id,
                Category = beer.Category,
                BeerStyle = beer.BeerStyle,
                AlcoholContent = beer.AlcoholContent,
                ElaborationDate = beer.ElaborationDate,
                BottleDate = beer.BottleDate,
                Malts = beer.Malts,
                Hops = beer.Hops,
                Yeast = beer.Yeast,
                Additives = beer.Additives,
                ParticpantId = beer.ParticpantId,
                ParticipantEmail = beer.ParticipantEmail,
                EntryInstructions = beer.EntryInstructions,
                ContestId = beer.ContestId,
                CreatedAt = beer.CreatedAt,
                ContestStatus = contest?.Status ?? ContestStatus.Draft
            };
        }

        private static string DetermineContestStatus(Contest? contest)
        {
            if (contest == null) return "Contest not found";
            
            var now = DateTime.UtcNow;
            
            if (now < contest.RegistrationStartDate)
                return "Registration not started";
            if (now >= contest.RegistrationStartDate && now <= contest.RegistrationEndDate)
                return "Registration open";
            if (now > contest.RegistrationEndDate && now < contest.ShipphingStartDate)
                return "Registration closed";
            if (now >= contest.ShipphingStartDate && now <= contest.ShipphingEndDate)
                return "Shipping period";
            if (now > contest.ShipphingEndDate)
                return "Contest in progress";
                
            return "Unknown";
        }
    }
}
