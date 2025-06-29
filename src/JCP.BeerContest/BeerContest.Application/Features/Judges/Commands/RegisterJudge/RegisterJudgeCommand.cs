using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace BeerContest.Application.Features.Judges.Commands.RegisterJudge
{
    public class RegisterJudgeCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Preferences { get; set; }
        public string BcjpId { get; set; }
        public string ContestId { get; set; }
        public string ContestName { get; set; }
    }
 
     public class RegisterJudgeCommandHandler : IRequestHandler<RegisterJudgeCommand, string>
    {
        private readonly IJudgeRepository _judgeRepository;

        public RegisterJudgeCommandHandler(IJudgeRepository judgeRepository)
        {
            _judgeRepository = judgeRepository;
        }

        public async Task<string> Handle(RegisterJudgeCommand request, CancellationToken cancellationToken)
        {
            var judge = new Judge
            {
                Id = Guid.NewGuid().ToString(), // Generate a new ID for the judge
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                Email = request.Email,
                Preferences = request.Preferences,
                BcjpId = request.BcjpId,
                ContestId = request.ContestId,
                ContestName = request.ContestName
            };

            return await _judgeRepository.CreateAsync(judge);
        }
    }

    public class RegisterJudgeCommandValidator : AbstractValidator<RegisterJudgeCommand>
    {
        public RegisterJudgeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}