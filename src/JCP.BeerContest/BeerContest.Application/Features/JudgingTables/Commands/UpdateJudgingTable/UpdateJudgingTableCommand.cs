using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Commands.UpdateJudgingTable
{
    public class UpdateJudgingTableCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
    }

    public class UpdateJudgingTableCommandHandler : IRequestHandler<UpdateJudgingTableCommand, Unit>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public UpdateJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<Unit> Handle(UpdateJudgingTableCommand request, CancellationToken cancellationToken)
        {
            var judgingTable = await _judgingTableRepository.GetByIdAsync(request.Id);
            
            if (judgingTable == null)
            {
                throw new Exception($"Judging table with ID {request.Id} not found");
            }

            judgingTable.Name = request.Name;
            judgingTable.JudgeIds = request.JudgeIds;
            judgingTable.BeerIds = request.BeerIds;
            judgingTable.UpdatedAt = DateTime.Now;

            await _judgingTableRepository.UpdateAsync(judgingTable);

            return Unit.Value;
        }
    }
}