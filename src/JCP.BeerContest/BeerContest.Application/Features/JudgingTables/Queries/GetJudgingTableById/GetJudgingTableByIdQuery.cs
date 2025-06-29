using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetJudgingTableById
{
    public class GetJudgingTableByIdQuery : IApiRequest<JudgingTable>
    {
        public required string Id { get; set; }
    }

    public class GetJudgingTableByIdQueryHandler : IApiRequestHandler<GetJudgingTableByIdQuery, JudgingTable>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetJudgingTableByIdQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<JudgingTable>> Handle(GetJudgingTableByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return ApiResponse<JudgingTable>.Failure("Judging table ID is required");
                }
                
                var judgingTable = await _judgingTableRepository.GetByIdAsync(request.Id);
                
                if (judgingTable == null)
                {
                    return ApiResponse<JudgingTable>.Failure($"Judging table with ID {request.Id} not found");
                }

                return ApiResponse<JudgingTable>.Success(judgingTable, "Judging table retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<JudgingTable>.Failure(
                    "Failed to retrieve judging table",
                    new List<string> { ex.Message });
            }
        }
    }
}