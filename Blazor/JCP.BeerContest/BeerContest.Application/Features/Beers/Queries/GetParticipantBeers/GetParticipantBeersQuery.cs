using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetParticipantBeers
{
    public class GetParticipantBeersQuery : IRequest<IEnumerable<BeerWithContestStatus>>
    {
        public string ParticipantEmail { get; set; }
    }

    public class GetParticipantBeersQueryHandler : IRequestHandler<GetParticipantBeersQuery, IEnumerable<BeerWithContestStatus>>
    {
        private readonly IBeerRepository _beerRepository;

        public GetParticipantBeersQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<IEnumerable<BeerWithContestStatus>> Handle(GetParticipantBeersQuery request, CancellationToken cancellationToken)
        {
            var beersWithContestStatus = await _beerRepository.GetByParticipantAsync(request.ParticipantEmail);
           
            return beersWithContestStatus;
        }
    }
}