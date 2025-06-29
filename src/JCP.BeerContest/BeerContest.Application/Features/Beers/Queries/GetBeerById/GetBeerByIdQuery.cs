using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Queries.GetBeerById
{
    public class GetBeerByIdQuery : IApiRequest<Beer>
    {
        public required string Id { get; set; }
    }

    public class GetBeerByIdQueryHandler : IApiRequestHandler<GetBeerByIdQuery, Beer>
    {
        private readonly IBeerRepository _beerRepository;

        public GetBeerByIdQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<ApiResponse<Beer>> Handle(GetBeerByIdQuery request, CancellationToken cancellationToken)
        {
            var beer = await _beerRepository.GetByIdAsync(request.Id);

            return ApiResponse<Beer>.Success(
                   beer,
                   $"Successfully retrieved");
        }
    }
}