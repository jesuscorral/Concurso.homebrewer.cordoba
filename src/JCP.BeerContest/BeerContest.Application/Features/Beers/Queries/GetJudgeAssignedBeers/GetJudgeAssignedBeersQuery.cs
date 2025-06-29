using BeerContest.Domain.Models;
using MediatR;

namespace BeerContest.Application.Features.Beers.Queries.GetJudgeAssignedBeers
{
    public class GetJudgeAssignedBeersQuery : IRequest<IEnumerable<Beer>>
    {
        public required string JudgeId { get; set; }
    }

    public class GetJudgeAssignedBeersQueryHandler : IRequestHandler<GetJudgeAssignedBeersQuery, IEnumerable<Beer>>
    {
        public GetJudgeAssignedBeersQueryHandler()
        {
        }

        public async Task<IEnumerable<Beer>> Handle(GetJudgeAssignedBeersQuery request, CancellationToken cancellationToken)
        { 
        //{
        //    // Verify that the user is a judge
        //    var user = await _userRepository.GetByIdAsync(request.JudgeId);
        //    if (user == null || user.Role != UserRole.Judge)
        //    {
        //        throw new Exception("User is not a judge");
        //    }

        //    // Get beers assigned to the judge
        //    // Note: The repository implementation ensures only public beer data is returned
        //    return await _beerRepository.GetAssignedToJudgeAsync(request.JudgeId);

        // TODO: Implement the logic to get beers assigned to the judge
            throw new NotImplementedException("GetJudgeAssignedBeersQueryHandler is not implemented yet");
        }
    }
}