using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Queries.GetParticipantBeersByContestId
{
    public class GetParticipantBeersByContestIdQuery : IApiRequest<IEnumerable<BeerWithContestStatus>>
    {
        public required string ParticipantEmail { get; set; }
        public required string ContestId { get; set; }
    }

    public class GetParticipantBeersQueryHandler : IApiRequestHandler<GetParticipantBeersByContestIdQuery, IEnumerable<BeerWithContestStatus>>
    {
        private readonly IBeerRepository _beerRepository;

        public GetParticipantBeersQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<ApiResponse<IEnumerable<BeerWithContestStatus>>> Handle(GetParticipantBeersByContestIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var beersWithContestStatus = await _beerRepository.GetByParticipantAndContestIdAsync(request.ParticipantEmail, request.ContestId);

                return ApiResponse<IEnumerable<BeerWithContestStatus>>.Success(
                   beersWithContestStatus,
                   $"Successfully retrieved {beersWithContestStatus.Count()} beers");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<BeerWithContestStatus>>.Failure(
                   "Failed to retrieve participant beers",
                   new List<string> { ex.Message });
            }
           
        }
    }
}