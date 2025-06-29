using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.AreAllBeersAssigned
{
    public class AreAllBeersAssignedQuery : IApiRequest<bool>
    {
        public required string ContestId { get; set; }
    }

    public class AreAllBeersAssignedQueryHandler : IApiRequestHandler<AreAllBeersAssignedQuery, bool>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public AreAllBeersAssignedQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<bool>> Handle(AreAllBeersAssignedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<bool>.Failure("Contest ID is required");
                }

                var areAllAssigned = await _judgingTableRepository.AreAllBeersAssignedAsync(request.ContestId);
                
                if (areAllAssigned)
                {
                    return ApiResponse<bool>.Success(true, "All beers are assigned to judging tables");
                }
                else
                {
                    return ApiResponse<bool>.Success(false, "Some beers are not assigned to judging tables");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Failed to check if all beers are assigned",
                    new List<string> { ex.Message });
            }
        }
    }
}