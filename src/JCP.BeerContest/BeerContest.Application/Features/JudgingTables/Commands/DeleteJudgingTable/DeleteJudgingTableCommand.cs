using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Commands.DeleteJudgingTable
{
    public class DeleteJudgingTableCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }

    public class DeleteJudgingTableCommandHandler : IRequestHandler<DeleteJudgingTableCommand, Unit>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public DeleteJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<Unit> Handle(DeleteJudgingTableCommand request, CancellationToken cancellationToken)
        {
            var judgingTable = await _judgingTableRepository.GetByIdAsync(request.Id);
            
            if (judgingTable == null)
            {
                throw new Exception($"Judging table with ID {request.Id} not found");
            }

            await _judgingTableRepository.DeleteAsync(request.Id);

            return Unit.Value;
        }
    }
}