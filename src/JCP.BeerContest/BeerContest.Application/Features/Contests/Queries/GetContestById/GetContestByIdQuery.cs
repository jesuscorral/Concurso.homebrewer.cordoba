using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Contests.Queries.GetContestById
{
    public class GetContestByIdQuery : IApiRequest<Contest>
    {
        public required string Id { get; set; }
    }

    public class GetContestByIdQueryHandler : IApiRequestHandler<GetContestByIdQuery, Contest>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestByIdQueryHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<ApiResponse<Contest>> Handle(GetContestByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return ApiResponse<Contest>.Failure("Contest ID is required");
                }
                
                var contest = await _contestRepository.GetByIdAsync(request.Id);
                
                if (contest == null)
                {
                    return ApiResponse<Contest>.Failure($"Contest with ID {request.Id} not found");
                }
                
                return ApiResponse<Contest>.Success(contest, "Contest retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Contest>.Failure(
                    "Failed to retrieve contest",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}