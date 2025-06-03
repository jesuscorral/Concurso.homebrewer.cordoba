using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Queries.GetAllContests
{
    public class GetAllContestsQuery : IRequest<IEnumerable<Contest>>
    {
        // No parameters needed for getting all contests
    }

    public class GetAllContestsQueryHandler : IRequestHandler<GetAllContestsQuery, IEnumerable<Contest>>
    {
        private readonly IContestRepository _contestRepository;

        public GetAllContestsQueryHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<IEnumerable<Contest>> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
        {
            return await _contestRepository.GetAllAsync();
        }
    }
}