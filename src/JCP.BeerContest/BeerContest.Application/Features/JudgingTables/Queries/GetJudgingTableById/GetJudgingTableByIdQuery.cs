using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetJudgingTableById
{
    public class GetJudgingTableByIdQuery : IRequest<JudgingTable>
    {
        public string Id { get; set; }
    }

    public class GetJudgingTableByIdQueryHandler : IRequestHandler<GetJudgingTableByIdQuery, JudgingTable>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetJudgingTableByIdQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<JudgingTable> Handle(GetJudgingTableByIdQuery request, CancellationToken cancellationToken)
        {
            var judgingTable = await _judgingTableRepository.GetByIdAsync(request.Id);
            
            if (judgingTable == null)
            {
                throw new Exception($"Judging table with ID {request.Id} not found");
            }

            return judgingTable;
        }
    }
}