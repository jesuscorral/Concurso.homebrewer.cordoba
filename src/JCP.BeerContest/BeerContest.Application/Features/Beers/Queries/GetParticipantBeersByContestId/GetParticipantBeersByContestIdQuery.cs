using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetParticipantBeersByContestId
{
    public class GetParticipantBeersByContestIdQuery : IRequest<IEnumerable<BeerWithContestStatus>>
    {
        public required string ParticipantEmail { get; set; }
        public required string ContestId { get; set; }
    }

    public class GetParticipantBeersQueryHandler : IRequestHandler<GetParticipantBeersByContestIdQuery, IEnumerable<BeerWithContestStatus>>
    {
        private readonly IBeerRepository _beerRepository;

        public GetParticipantBeersQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<IEnumerable<BeerWithContestStatus>> Handle(GetParticipantBeersByContestIdQuery request, CancellationToken cancellationToken)
        {
            var beersWithContestStatus = await _beerRepository.GetByParticipantAndContestIdAsync(request.ParticipantEmail, request.ContestId);
           
            return beersWithContestStatus;
        }
    }
}