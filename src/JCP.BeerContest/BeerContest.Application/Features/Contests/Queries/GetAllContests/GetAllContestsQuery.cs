using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Contests.Queries.GetAllContests
{
    public class GetAllContestsQuery : IApiRequest<IEnumerable<Contest>>
    {
        // No parameters needed for getting all contests
    }

    public class GetAllContestsQueryHandler : IApiRequestHandler<GetAllContestsQuery, IEnumerable<Contest>>
    {
        private readonly IContestRepository _contestRepository;

        public GetAllContestsQueryHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<ApiResponse<IEnumerable<Contest>>> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contests = await _contestRepository.GetAllAsync();
                
                if (contests == null || !contests.Any())
                {
                    return ApiResponse<IEnumerable<Contest>>.Success(new List<Contest>(), "No contests found");
                }
                
                return ApiResponse<IEnumerable<Contest>>.Success(
                    contests, 
                    $"Successfully retrieved {contests.Count()} contests");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Contest>>.Failure(
                    "Failed to retrieve contests", 
                    new List<string> { ex.Message });
            }
        }
    }
}