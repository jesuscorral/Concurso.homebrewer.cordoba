using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetallBeeryByContest
{
    public class GetallBeeryByContestCommand : IRequest<IEnumerable<Beer>>
    {
        public required string ContestId { get; set; }
    }

    public class GetallBeeryByContestCommandHandler : IRequestHandler<GetallBeeryByContestCommand, IEnumerable<Beer>>
    {
        private readonly IBeerRepository _beerRepository;
        public GetallBeeryByContestCommandHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }
        public async Task<IEnumerable<Beer>> Handle(GetallBeeryByContestCommand request, CancellationToken cancellationToken)
        {
            return await _beerRepository.GetByContestAsync(request.ContestId);
        }
    }
}
