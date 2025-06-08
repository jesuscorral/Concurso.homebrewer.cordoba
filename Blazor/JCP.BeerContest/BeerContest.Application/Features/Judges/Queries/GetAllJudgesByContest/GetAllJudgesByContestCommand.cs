using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Judges.Queries.GetAllJudgesByContest
{
    public class GetAllJudgesByContestCommand : IRequest<IEnumerable<Judge>>
    {
        public string ContestId { get; set; }
    }

    public class GetAllJudgesCommandHandler : IRequestHandler<GetAllJudgesByContestCommand, IEnumerable<Judge>>
    {
        private readonly IJudgeRepository _judgeRepository;
        public GetAllJudgesCommandHandler(IJudgeRepository judgeRepository)
        {
            _judgeRepository = judgeRepository;
        }
        public async Task<IEnumerable<Judge>> Handle(GetAllJudgesByContestCommand request, CancellationToken cancellationToken)
        {
            return await _judgeRepository.GetAllByContestAsync(request.ContestId);
        }
    }
}
