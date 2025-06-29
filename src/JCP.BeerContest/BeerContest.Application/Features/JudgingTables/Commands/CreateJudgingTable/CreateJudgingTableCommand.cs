using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Commands.CreateJudgingTable
{
    public class CreateJudgingTableCommand : IApiRequest<string>
    {
        public required string Name { get; set; }
        public required string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
    }

    public class CreateJudgingTableCommandHandler : IApiRequestHandler<CreateJudgingTableCommand, string>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public CreateJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<string>> Handle(CreateJudgingTableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return ApiResponse<string>.Failure("Judging table name is required");
                }

                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<string>.Failure("Contest ID is required");
                }

                var judgingTable = new JudgingTable
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    ContestId = request.ContestId,
                    JudgeIds = request.JudgeIds,
                    BeerIds = request.BeerIds,
                    CreatedAt = DateTime.UtcNow
                };

                var id = await _judgingTableRepository.CreateAsync(judgingTable);
                return ApiResponse<string>.Success(id, "Judging table created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(
                    "Failed to create judging table", 
                    new List<string> { ex.Message });
            }
        }
    }
}