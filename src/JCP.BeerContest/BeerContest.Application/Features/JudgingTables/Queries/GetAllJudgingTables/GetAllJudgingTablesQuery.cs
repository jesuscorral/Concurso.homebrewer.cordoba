using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetAllJudgingTables
{
    public class GetAllJudgingTablesQuery : IApiRequest<IEnumerable<JudgingTable>>
    {
        public required string ContestId { get; set; }
    }

    public class GetAllJudgingTablesQueryHandler : IApiRequestHandler<GetAllJudgingTablesQuery, IEnumerable<JudgingTable>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetAllJudgingTablesQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<IEnumerable<JudgingTable>>> Handle(GetAllJudgingTablesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<IEnumerable<JudgingTable>>.Failure("Contest ID is required");
                }

                var tables = await _judgingTableRepository.GetByContestIdAsync(request.ContestId);
                
                if (tables == null || !tables.Any())
                {
                    return ApiResponse<IEnumerable<JudgingTable>>.Success(
                        new List<JudgingTable>(), 
                        "No judging tables found for this contest");
                }
                
                return ApiResponse<IEnumerable<JudgingTable>>.Success(
                    tables,
                    $"Successfully retrieved {tables.Count()} judging tables");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<JudgingTable>>.Failure(
                    "Failed to retrieve judging tables",
                    new List<string> { ex.Message });
            }
        }
    }
}