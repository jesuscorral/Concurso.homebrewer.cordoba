using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Queries.GetActiveContests
{
    public class GetActiveContestsQuery : IRequest<IEnumerable<Contest>>
    {
    }

    public class GetActiveContestsQueryHandler : IRequestHandler<GetActiveContestsQuery, IEnumerable<Contest>>
    {
        private readonly IContestRepository _contestRepository;

        public GetActiveContestsQueryHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<IEnumerable<Contest>> Handle(GetActiveContestsQuery request, CancellationToken cancellationToken)
        {
            return await _contestRepository.GetActiveContestsAsync();
        }
    }
}