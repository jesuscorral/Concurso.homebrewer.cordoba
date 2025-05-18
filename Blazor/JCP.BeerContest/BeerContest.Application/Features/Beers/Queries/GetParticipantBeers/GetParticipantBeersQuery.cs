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
        public string ParticipantEmail { get; set; }
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
            var beers = await _beerRepository.GetByParticipantAsync(request.ParticipantEmail);
           
            
            return beers;
        }
    }
}