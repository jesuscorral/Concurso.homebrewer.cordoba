using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.AssignBeersToJudge
{
    public class AssignBeersToJudgeCommand : IRequest<bool>
    {
        public required string JudgeId { get; set; }
        public List<string> BeerIds { get; set; } = new List<string>();
    }

    public class AssignBeersToJudgeCommandHandler : IRequestHandler<AssignBeersToJudgeCommand, bool>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IUserRepository _userRepository;

        public AssignBeersToJudgeCommandHandler(
            IBeerRepository beerRepository,
            IUserRepository userRepository)
        {
            _beerRepository = beerRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AssignBeersToJudgeCommand request, CancellationToken cancellationToken)
        {
            // Verify that the user is a judge
            var user = await _userRepository.GetByIdAsync(request.JudgeId);
            if (user == null)
            {
                throw new Exception($"User with ID {request.JudgeId} not found");
            }

            if (!user.Roles.Contains(UserRole.Judge))
            {
                throw new Exception($"User with ID {request.JudgeId} is not a judge");
            }

            // Assign beers to the judge
            await _beerRepository.AssignBeersToJudgeAsync(request.JudgeId, request.BeerIds);
            
            return true;
        }
    }

    public class AssignBeersToJudgeCommandValidator : IValidator<AssignBeersToJudgeCommand>
    {
        private readonly IUserRepository _userRepository;

        public AssignBeersToJudgeCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<AssignBeersToJudgeCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.JudgeId))
            {
                result.Errors.Add(new ValidationFailure("JudgeId", "Judge ID is required"));
            }
            else
            {
                // Verify that the user is a judge
                var user = await _userRepository.GetByIdAsync(command.JudgeId);
                if (user == null)
                {
                    result.Errors.Add(new ValidationFailure("JudgeId", $"User with ID {command.JudgeId} not found"));
                }
                else if (!user.Roles.Contains(UserRole.Judge))
                {
                    result.Errors.Add(new ValidationFailure("JudgeId", $"User with ID {command.JudgeId} is not a judge"));
                }
            }

            if (command.BeerIds == null || command.BeerIds.Count == 0)
            {
                result.Errors.Add(new ValidationFailure("BeerIds", "At least one beer must be assigned"));
            }

            return result;
        }
    }
}