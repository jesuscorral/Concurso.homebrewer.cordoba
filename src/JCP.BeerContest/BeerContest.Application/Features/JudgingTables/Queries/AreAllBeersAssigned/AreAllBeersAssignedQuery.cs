using BeerContest.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.AreAllBeersAssigned
{
    public class AreAllBeersAssignedQuery : IRequest<bool>
    {
        public string ContestId { get; set; }
    }

    public class AreAllBeersAssignedQueryHandler : IRequestHandler<AreAllBeersAssignedQuery, bool>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public AreAllBeersAssignedQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<bool> Handle(AreAllBeersAssignedQuery request, CancellationToken cancellationToken)
        {
            return await _judgingTableRepository.AreAllBeersAssignedAsync(request.ContestId);
        }
    }
}