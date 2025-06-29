using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetUnassignedBeers
{
    public class GetUnassignedBeersQuery : IRequest<IEnumerable<Beer>>
    {
        public required string ContestId { get; set; }
    }

    public class GetUnassignedBeersQueryHandler : IRequestHandler<GetUnassignedBeersQuery, IEnumerable<Beer>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetUnassignedBeersQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<IEnumerable<Beer>> Handle(GetUnassignedBeersQuery request, CancellationToken cancellationToken)
        {
            return await _judgingTableRepository.GetUnassignedBeersAsync(request.ContestId);
        }
    }
}