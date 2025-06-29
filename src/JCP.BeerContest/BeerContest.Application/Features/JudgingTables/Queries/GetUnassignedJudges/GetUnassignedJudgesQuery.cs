using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetUnassignedJudges
{
    public class GetUnassignedJudgesQuery : IApiRequest<IEnumerable<Judge>>
    {
        public required string ContestId { get; set; }
    }

    public class GetUnassignedJudgesQueryHandler : IApiRequestHandler<GetUnassignedJudgesQuery, IEnumerable<Judge>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetUnassignedJudgesQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<IEnumerable<Judge>>> Handle(GetUnassignedJudgesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<IEnumerable<Judge>>.Failure("Contest ID is required");
                }

                var judges = await _judgingTableRepository.GetUnassignedJudgesAsync(request.ContestId);

                if (judges == null || !judges.Any())
                {
                    return ApiResponse<IEnumerable<Judge>>.Success(
                        new List<Judge>(),
                        "No unassigned judges found for this contest");
                }

                return ApiResponse<IEnumerable<Judge>>.Success(
                    judges,
                    $"Successfully retrieved {judges.Count()} unassigned judges");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Judge>>.Failure(
                    "Failed to retrieve unassigned judges",
                    new List<string> { ex.Message });
            }
        }
    }
}