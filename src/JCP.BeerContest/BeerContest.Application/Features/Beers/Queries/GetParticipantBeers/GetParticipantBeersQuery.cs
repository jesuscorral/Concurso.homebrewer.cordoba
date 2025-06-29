using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Queries.GetParticipantBeers
{
    public class GetParticipantBeersQuery : IApiRequest<IEnumerable<BeerWithContestStatus>>
    {
        public required string ParticipantEmail { get; set; }
    }

    public class GetParticipantBeersQueryHandler : IApiRequestHandler<GetParticipantBeersQuery, IEnumerable<BeerWithContestStatus>>
    {
        private readonly IBeerRepository _beerRepository;

        public GetParticipantBeersQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<ApiResponse<IEnumerable<BeerWithContestStatus>>> Handle(GetParticipantBeersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ParticipantEmail))
                {
                    return ApiResponse<IEnumerable<BeerWithContestStatus>>.Failure("Participant email is required");
                }

                var beersWithContestStatus = await _beerRepository.GetByParticipantAsync(request.ParticipantEmail);

                if (beersWithContestStatus == null || !beersWithContestStatus.Any())
                {
                    return ApiResponse<IEnumerable<BeerWithContestStatus>>.Success(
                        new List<BeerWithContestStatus>(), 
                        "No beers found for this participant");
                }

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