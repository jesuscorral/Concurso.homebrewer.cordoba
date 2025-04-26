using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetParticipantBeers
{
    public class GetParticipantBeersQuery : IRequest<IEnumerable<Beer>>
    {
        public string ParticipantId { get; set; }
        public string ContestId { get; set; }
    }

    public class GetParticipantBeersQueryHandler : IRequestHandler<GetParticipantBeersQuery, IEnumerable<Beer>>
    {
        private readonly IBeerRepository _beerRepository;

        public GetParticipantBeersQueryHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<IEnumerable<Beer>> Handle(GetParticipantBeersQuery request, CancellationToken cancellationToken)
        {
            var beers = await _beerRepository.GetByBrewerAsync(request.ParticipantId);
            
            // If a contest ID is provided, filter by contest
            if (!string.IsNullOrEmpty(request.ContestId))
            {
                var filteredBeers = new List<Beer>();
                foreach (var beer in beers)
                {
                    if (beer.ContestId == request.ContestId)
                    {
                        filteredBeers.Add(beer);
                    }
                }
                return filteredBeers;
            }
            
            return beers;
        }
    }
}