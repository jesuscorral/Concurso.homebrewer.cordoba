using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetBeerById
{
    public class GetBeerByIdQuery : IRequest<Beer>
    {
        public required string Id { get; set; }
    }

    public class GetBeerByIdQueryHandler : IRequestHandler<GetBeerByIdQuery, Beer>
    {
        private readonly IBeerRepository _beerRepository;

        public GetBeerByIdQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<Beer> Handle(GetBeerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _beerRepository.GetByIdAsync(request.Id);
        }
    }
}