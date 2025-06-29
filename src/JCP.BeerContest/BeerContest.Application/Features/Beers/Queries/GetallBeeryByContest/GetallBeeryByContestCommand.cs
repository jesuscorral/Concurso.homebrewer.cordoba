using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Queries.GetallBeeryByContest
{
    public class GetallBeeryByContestCommand : IApiRequest<IEnumerable<Beer>>
    {
        public required string ContestId { get; set; }
    }

    public class GetallBeeryByContestCommandHandler : IApiRequestHandler<GetallBeeryByContestCommand, IEnumerable<Beer>>
    {
        private readonly IBeerRepository _beerRepository;
        public GetallBeeryByContestCommandHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }
        public async Task<ApiResponse<IEnumerable<Beer>>> Handle(GetallBeeryByContestCommand request, CancellationToken cancellationToken)
        {
            var beers = await _beerRepository.GetByContestAsync(request.ContestId);

            return ApiResponse<IEnumerable<Beer>>.Success(
                   beers,
                   $"Successfully retrieved {beers.Count()} beers");
        }
    }
}