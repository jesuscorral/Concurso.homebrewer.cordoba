using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RateBeer
{
    public class RateBeerCommand : IRequest<bool>
    {
        public string BeerId { get; set; }
        public string JudgeId { get; set; }
        public int AppearanceScore { get; set; }
        public int AromaScore { get; set; }
        public int FlavorScore { get; set; }
        public int MouthfeelScore { get; set; }
        public int OverallImpressionScore { get; set; }
        public string Comments { get; set; }
    }

    public class RateBeerCommandHandler : IRequestHandler<RateBeerCommand, bool>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IUserRepository _userRepository;

        public RateBeerCommandHandler(
            IBeerRepository beerRepository,
            IUserRepository userRepository)
        {
            _beerRepository = beerRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RateBeerCommand request, CancellationToken cancellationToken)
        {
            // Verify that the user is a judge
            var user = await _userRepository.GetByIdAsync(request.JudgeId);
            if (user == null)
            {
                throw new Exception($"User with ID {request.JudgeId} not found");
            }

            if (user.Role != UserRole.Judge)
            {
                throw new Exception($"User with ID {request.JudgeId} is not a judge");
            }

            // Verify that the beer is assigned to the judge
            var assignedBeers = await _beerRepository.GetAssignedToJudgeAsync(request.JudgeId);
            var beer = assignedBeers.FirstOrDefault(b => b.Id == request.BeerId);
            if (beer == null)
            {
                throw new Exception($"Beer with ID {request.BeerId} is not assigned to judge with ID {request.JudgeId}");
            }

            // Create the rating
            var rating = new BeerRating
            {
                BeerId = request.BeerId,
                JudgeId = request.JudgeId,
                AppearanceScore = request.AppearanceScore,
                AromaScore = request.AromaScore,
                FlavorScore = request.FlavorScore,
                MouthfeelScore = request.MouthfeelScore,
                OverallImpressionScore = request.OverallImpressionScore,
                Comments = request.Comments,
                RatedAt = DateTime.UtcNow
            };

            // Add the rating to the beer
            await _beerRepository.AddRatingAsync(request.BeerId, rating);
            
            return true;
        }
    }

    public class RateBeerCommandValidator : IValidator<RateBeerCommand>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IUserRepository _userRepository;

        public RateBeerCommandValidator(
            IBeerRepository beerRepository,
            IUserRepository userRepository)
        {
            _beerRepository = beerRepository;
            _userRepository = userRepository;
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RateBeerCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.BeerId))
            {
                result.Errors.Add(new ValidationFailure("BeerId", "Beer ID is required"));
            }

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
                else if (user.Role != UserRole.Judge)
                {
                    result.Errors.Add(new ValidationFailure("JudgeId", $"User with ID {command.JudgeId} is not a judge"));
                }
                else
                {
                    // Verify that the beer is assigned to the judge
                    var assignedBeers = await _beerRepository.GetAssignedToJudgeAsync(command.JudgeId);
                    var beer = assignedBeers.FirstOrDefault(b => b.Id == command.BeerId);
                    if (beer == null)
                    {
                        result.Errors.Add(new ValidationFailure("BeerId", $"Beer with ID {command.BeerId} is not assigned to judge with ID {command.JudgeId}"));
                    }
                }
            }

            // Validate scores (1-5 range)
            if (command.AppearanceScore < 1 || command.AppearanceScore > 5)
            {
                result.Errors.Add(new ValidationFailure("AppearanceScore", "Appearance score must be between 1 and 5"));
            }

            if (command.AromaScore < 1 || command.AromaScore > 5)
            {
                result.Errors.Add(new ValidationFailure("AromaScore", "Aroma score must be between 1 and 5"));
            }

            if (command.FlavorScore < 1 || command.FlavorScore > 5)
            {
                result.Errors.Add(new ValidationFailure("FlavorScore", "Flavor score must be between 1 and 5"));
            }

            if (command.MouthfeelScore < 1 || command.MouthfeelScore > 5)
            {
                result.Errors.Add(new ValidationFailure("MouthfeelScore", "Mouthfeel score must be between 1 and 5"));
            }

            if (command.OverallImpressionScore < 1 || command.OverallImpressionScore > 5)
            {
                result.Errors.Add(new ValidationFailure("OverallImpressionScore", "Overall impression score must be between 1 and 5"));
            }

            return result;
        }
    }
}