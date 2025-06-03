using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Queries.GetContestById
{
    public class GetContestByIdQuery : IRequest<Contest>
    {
        public string Id { get; set; }
    }

    public class GetContestByIdQueryHandler : IRequestHandler<GetContestByIdQuery, Contest>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestByIdQueryHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<Contest> Handle(GetContestByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contestRepository.GetByIdAsync(request.Id);
        }
    }
}