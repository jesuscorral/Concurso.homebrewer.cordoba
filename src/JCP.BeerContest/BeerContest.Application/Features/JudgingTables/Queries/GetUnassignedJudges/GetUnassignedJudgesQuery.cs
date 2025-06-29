using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetUnassignedJudges
{
    public class GetUnassignedJudgesQuery : IRequest<IEnumerable<Judge>>
    {
        public required string ContestId { get; set; }
    }

    public class GetUnassignedJudgesQueryHandler : IRequestHandler<GetUnassignedJudgesQuery, IEnumerable<Judge>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetUnassignedJudgesQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<IEnumerable<Judge>> Handle(GetUnassignedJudgesQuery request, CancellationToken cancellationToken)
        {
            return await _judgingTableRepository.GetUnassignedJudgesAsync(request.ContestId);
        }
    }
}