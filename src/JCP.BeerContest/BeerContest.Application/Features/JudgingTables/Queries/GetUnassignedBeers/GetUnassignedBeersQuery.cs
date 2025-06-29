using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.JudgingTables.Queries.GetUnassignedBeers
{
    public class GetUnassignedBeersQuery : IApiRequest<IEnumerable<Beer>>
    {
        public required string ContestId { get; set; }
    }

    public class GetUnassignedBeersQueryHandler : IApiRequestHandler<GetUnassignedBeersQuery, IEnumerable<Beer>>
    {
        private readonly IJudgingTableRepository _judgingTableRepository;

        public GetUnassignedBeersQueryHandler(IJudgingTableRepository judgingTableRepository)
        {
            _judgingTableRepository = judgingTableRepository;
        }

        public async Task<ApiResponse<IEnumerable<Beer>>> Handle(GetUnassignedBeersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<IEnumerable<Beer>>.Failure("Contest ID is required");
                }

                var beers = await _judgingTableRepository.GetUnassignedBeersAsync(request.ContestId);

                if (beers == null || !beers.Any())
                {
                    return ApiResponse<IEnumerable<Beer>>.Success(
                        new List<Beer>(),
                        "No unassigned beers found for this contest");
                }

                return ApiResponse<IEnumerable<Beer>>.Success(
                    beers,
                    $"Successfully retrieved {beers.Count()} unassigned beers");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Beer>>.Failure(
                    "Failed to retrieve unassigned beers",
                    new List<string> { ex.Message });
            }
        }
    }
}