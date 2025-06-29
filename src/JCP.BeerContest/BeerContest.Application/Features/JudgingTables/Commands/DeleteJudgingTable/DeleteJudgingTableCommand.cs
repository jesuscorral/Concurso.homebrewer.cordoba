using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Commands.DeleteJudgingTable
{
    public class DeleteJudgingTableCommand : IApiRequest<bool>
    {
        public required string Id { get; set; }
    }

    public class DeleteJudgingTableCommandHandler : IApiRequestHandler<DeleteJudgingTableCommand, bool>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public DeleteJudgingTableCommandHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteJudgingTableCommand request, CancellationToken cancellationToken)
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

                await _judgingTableRepository.DeleteAsync(request.Id);
                return ApiResponse<bool>.Success(true, "Judging table deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Failed to delete judging table",
                    new List<string> { ex.Message });
            }
        }
    }
}