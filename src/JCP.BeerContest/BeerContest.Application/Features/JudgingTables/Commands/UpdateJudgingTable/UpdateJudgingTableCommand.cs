using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Commands.UpdateJudgingTable
{
    public class UpdateJudgingTableCommand : IApiRequest<bool>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
    }

    public class UpdateJudgingTableCommandHandler : IApiRequestHandler<UpdateJudgingTableCommand, bool>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public UpdateJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateJudgingTableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return ApiResponse<bool>.Failure("Judging table ID is required");
                }
                
                var judgingTable = await _judgingTableRepository.GetByIdAsync(request.Id);
                
                if (judgingTable == null)
                {
                    return ApiResponse<bool>.Failure($"Judging table with ID {request.Id} not found");
                }

                judgingTable.Name = request.Name;
                judgingTable.JudgeIds = request.JudgeIds;
                judgingTable.BeerIds = request.BeerIds;
                judgingTable.UpdatedAt = DateTime.UtcNow;

                await _judgingTableRepository.UpdateAsync(judgingTable);

                return ApiResponse<bool>.Success(true, "Judging table updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Failed to update judging table",
                    new List<string> { ex.Message });
            }
        }
    }
}