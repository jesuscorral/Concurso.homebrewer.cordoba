using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetAllJudgingTables
{
    public class GetAllJudgingTablesQuery : IRequest<IEnumerable<JudgingTable>>
    {
        public required string ContestId { get; set; }
    }

    public class GetAllJudgingTablesQueryHandler : IRequestHandler<GetAllJudgingTablesQuery, IEnumerable<JudgingTable>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetAllJudgingTablesQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<IEnumerable<JudgingTable>> Handle(GetAllJudgingTablesQuery request, CancellationToken cancellationToken)
        {
            return await _judgingTableRepository.GetByContestIdAsync(request.ContestId);
        }
    }
}