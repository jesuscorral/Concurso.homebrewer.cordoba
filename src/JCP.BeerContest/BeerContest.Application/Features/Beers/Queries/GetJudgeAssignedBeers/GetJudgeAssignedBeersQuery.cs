using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Beers.Queries.GetJudgeAssignedBeers
{
    public class GetJudgeAssignedBeersQuery : IApiRequest<IEnumerable<Beer>>
    {
        public required string JudgeId { get; set; }
    }

    public class GetJudgeAssignedBeersQueryHandler : IApiRequestHandler<GetJudgeAssignedBeersQuery, IEnumerable<Beer>>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IUserRepository _userRepository;
        
        public GetJudgeAssignedBeersQueryHandler(
            IBeerRepository beerRepository,
            IUserRepository userRepository)
        {
            _beerRepository = beerRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<IEnumerable<Beer>>> Handle(GetJudgeAssignedBeersQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(request.JudgeId))
                {
                    return ApiResponse<IEnumerable<Beer>>.Failure("Judge ID is required");
                }

                // Verify that the user is a judge
                var user = await _userRepository.GetByIdAsync(request.JudgeId);
                if (user == null)
                {
                    return ApiResponse<IEnumerable<Beer>>.Failure($"User with ID {request.JudgeId} not found");
                }

                if (!user.Roles.Contains(UserRole.Judge))
                {
                    return ApiResponse<IEnumerable<Beer>>.Failure($"User with ID {request.JudgeId} is not a judge");
                }

                // TODO: Implement the logic to get beers assigned to the judge
                // For now, we'll return a failure response
                return ApiResponse<IEnumerable<Beer>>.Failure(
                    "The feature to get beers assigned to a judge is not yet implemented",
                    new List<string> { "This functionality will be available in a future update" });
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Beer>>.Failure(
                    "Failed to retrieve beers assigned to judge", 
                    new List<string> { ex.Message });
            }
        }
    }
}