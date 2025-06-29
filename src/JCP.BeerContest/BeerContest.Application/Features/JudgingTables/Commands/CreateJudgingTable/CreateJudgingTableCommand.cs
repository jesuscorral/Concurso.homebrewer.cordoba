using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.JudgingTables.Commands.CreateJudgingTable
{
    public class CreateJudgingTableCommand : IRequest<string>
    {
        public required string Name { get; set; }
        public required string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
    }

    public class CreateJudgingTableCommandHandler : IRequestHandler<CreateJudgingTableCommand, string>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public CreateJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<string> Handle(CreateJudgingTableCommand request, CancellationToken cancellationToken)
        {
            var judgingTable = new JudgingTable
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                ContestId = request.ContestId,
                JudgeIds = request.JudgeIds,
                BeerIds = request.BeerIds,
                CreatedAt = DateTime.Now
            };

            return await _judgingTableRepository.CreateAsync(judgingTable);
        }
    }
}