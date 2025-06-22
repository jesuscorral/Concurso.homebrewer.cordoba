using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class BeerRepository : IBeerRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "beers";
        private const string UserCollectionName = "users";
        private readonly IContestRepository _contestRepository;

        public BeerRepository(BeerContestContext firestoreContext, IContestRepository contestRepository)
        {
            _firestoreContext = firestoreContext;
            _contestRepository = contestRepository;
        }

        public async Task<Beer> GetByIdAsync(string id)
        {
            var firestoreBeer = await _firestoreContext.GetDocumentAsync<FirestoreBeer>(CollectionName, id);
            return firestoreBeer?.ToBeer();
        }

        public async Task<IEnumerable<Beer>> GetAllAsync()
        {
            var firestoreBeers = await _firestoreContext.GetCollectionAsync<FirestoreBeer>(CollectionName);
            return firestoreBeers.Select(fb => fb.ToBeer());
        }

        public async Task<IEnumerable<Beer>> GetByContestAsync(string contestId)
        {
            var query = await _firestoreContext.CreateQueryAsync(CollectionName);
            query = query.WhereEqualTo("ContestId", contestId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreBeer>().ToBeer())
                .ToList();
        }

        public async Task<IEnumerable<BeerWithContestStatus>> GetByParticipantAsync(string participantEmail)
        {
            // Get beers by participant email
            Query query = await _firestoreContext.CreateQueryAsync(CollectionName);
            query = query.WhereEqualTo("ParticipantEmail", participantEmail);


            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            var beers = querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreBeer>().ToBeer())
                .ToList();

            // Get unique contestIds from beers
            var contestIds = beers.Select(b => b.ContestId).Distinct().ToList();

            // Prepare a dictionary to hold contestId -> status
            var contestStatuses = new Dictionary<string, ContestStatus>();

            var contests = await _contestRepository.GetAllAsync();

            // Enrich Beer objects with contest status
            var beersWithStatus = beers.Select(b =>
            {
                var contest = contests.FirstOrDefault(x => x.Id.Equals(b.ContestId, StringComparison.Ordinal));
                return new BeerWithContestStatus
                {
                    Id = b.Id,
                    Category = b.Category,
                    BeerStyle = b.BeerStyle,
                    AlcoholContent = b.AlcoholContent,
                    ElaborationDate = b.ElaborationDate,
                    BottleDate = b.BottleDate,
                    Malts = b.Malts,
                    Hops = b.Hops,
                    Yeast = b.Yeast,
                    Additives = b.Additives,
                    ParticpantId = b.ParticpantId,
                    ParticipantEmail = b.ParticipantEmail,
                    EntryInstructions = b.EntryInstructions,
                    ContestId = b.ContestId,
                    CreatedAt = b.CreatedAt,
                    ContestStatus = contest?.Status ?? ContestStatus.Draft // Handle null contest gracefully
                };
            }).ToList();

            return beersWithStatus;
        }

        public async Task<IEnumerable<BeerWithContestStatus>> GetByParticipantAndContestIdAsync(string participantEmail, string contestId)
        {
            // Get beers by participant email
            var query = await _firestoreContext.CreateQueryAsync(CollectionName);
            query = query.WhereEqualTo("ParticipantEmail", participantEmail)
                .WhereEqualTo("ContestId", contestId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            var beers =  querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreBeer>().ToBeer())
                .ToList();

            var constest = await _contestRepository.GetByIdAsync(contestId);

            // Enrich Beer objects with contest status
            var beersWithStatus = beers.Select(b => new BeerWithContestStatus
            {
                Id = b.Id,
                Category = b.Category,
                BeerStyle = b.BeerStyle,
                AlcoholContent = b.AlcoholContent,
                ElaborationDate = b.ElaborationDate,
                BottleDate = b.BottleDate,
                Malts = b.Malts,
                Hops = b.Hops,
                Yeast = b.Yeast,
                Additives = b.Additives,
                ParticpantId = b.ParticpantId,
                ParticipantEmail = b.ParticipantEmail,
                EntryInstructions = b.EntryInstructions,
                ContestId = b.ContestId,
                CreatedAt = b.CreatedAt,
                ContestStatus = constest.Status
            }).ToList();

            return beersWithStatus;

        }


        //public async Task<IEnumerable<Beer>> GetAssignedToJudgeAsync(string judgeId)
        //{
        //    // Get the judge user document
        //    var judge = await _firestoreContext.GetDocumentAsync<User>(UserCollectionName, judgeId);

        //    if (judge == null || judge.AssignedBeersForJudging == null || !judge.AssignedBeersForJudging.Any())
        //    {
        //        return new List<Beer>();
        //    }

        //    // Get all beers assigned to this judge
        //    List<Beer> assignedBeers = new List<Beer>();
        //    foreach (var beer in judge.AssignedBeersForJudging)
        //    {
        //        var beerData = await GetByIdAsync(beer.Id);
        //        if (beerData != null)
        //        {
        //            // Return only public data for judges
        //            assignedBeers.Add(beerData.GetPublicData());
        //        }
        //    }

        //    return assignedBeers;
        //}

        public async Task<int> GetBrewerBeerCountAsync(string brewerId, string contestId)
        {
            Query query = await _firestoreContext.CreateQueryAsync(CollectionName);
            query = query.WhereEqualTo("BrewerId", brewerId)
                .WhereEqualTo("ContestId", contestId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents.Count;
        }

        public async Task<string> CreateAsync(Beer beer)
        {
            var firestoreBeer = FirestoreBeer.FromBeer(beer);
            string id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreBeer);
            
            return id;
        }

        public Task UpdateAsync(Beer beer)
        {
            var firestoreBeer = FirestoreBeer.FromBeer(beer);
            return _firestoreContext.SetDocumentAsync(CollectionName, beer.Id, firestoreBeer);
        }

        public Task DeleteAsync(string id)
        {
            return _firestoreContext.DeleteDocumentAsync(CollectionName, id);
        }

        //public async Task AddRatingAsync(string beerId, BeerRating rating)
        //{
        //    var beer = await GetByIdAsync(beerId);
        //    if (beer == null)
        //    {
        //        throw new ArgumentException($"Beer with ID {beerId} not found");
        //    }

        //    rating.Id = Guid.NewGuid().ToString();
        //    rating.BeerId = beerId;
        //    rating.RatedAt = DateTime.UtcNow;

        //    if (beer.Ratings == null)
        //    {
        //        beer.Ratings = new List<BeerRating>();
        //    }

        //    beer.Ratings.Add(rating);
        //    await UpdateAsync(beer);
        //}

        //public async Task UpdateRatingAsync(BeerRating rating)
        //{
        //    var beer = await GetByIdAsync(rating.BeerId);
        //    if (beer == null)
        //    {
        //        throw new ArgumentException($"Beer with ID {rating.BeerId} not found");
        //    }

        //    var existingRating = beer.Ratings?.FirstOrDefault(r => r.Id == rating.Id);
        //    if (existingRating == null)
        //    {
        //        throw new ArgumentException($"Rating with ID {rating.Id} not found");
        //    }

        //    // Update the rating
        //    int index = beer.Ratings.IndexOf(existingRating);
        //    beer.Ratings[index] = rating;

        //    await UpdateAsync(beer);
        //}

        public async Task AssignBeersToJudgeAsync(string judgeId, IEnumerable<string> beerIds)
        {
            var judge = await _firestoreContext.GetDocumentAsync<User>(UserCollectionName, judgeId);
            if (judge == null)
            {
                throw new ArgumentException($"Judge with ID {judgeId} not found");
            }

            if (!judge.Roles.Contains(UserRole.Judge))
            {
                throw new ArgumentException($"User with ID {judgeId} is not a judge");
            }

            // Get the beers to assign
            List<Beer> beersToAssign = new List<Beer>();
            foreach (var beerId in beerIds)
            {
                var beer = await GetByIdAsync(beerId);
                if (beer != null)
                {
                    beersToAssign.Add(beer);
                }
            }

            // Assign the beers to the judge
            judge.AssignedBeersForJudging = beersToAssign;
            await _firestoreContext.SetDocumentAsync(UserCollectionName, judgeId, judge);
        }
    }
}