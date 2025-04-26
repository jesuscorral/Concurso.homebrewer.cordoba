using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RegisterBeer
{
    public class RegisterBeerCommand : IRequest<string>
    {
        // Beer information
        public string Name { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
        public double AlcoholByVolume { get; set; }
        public string Color { get; set; }
        public string Aroma { get; set; }
        public string Flavor { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public string BrewingProcess { get; set; }
        
        // Brewer information
        public string BrewerId { get; set; }
        public string BrewerName { get; set; }
        public string BrewerEmail { get; set; }
        public string BrewerPhone { get; set; }
        
        // Contest information
        public string ContestId { get; set; }
    }

    public class RegisterBeerCommandHandler : IRequestHandler<RegisterBeerCommand, string>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IContestRepository _contestRepository;

        public RegisterBeerCommandHandler(
            IBeerRepository beerRepository,
            IContestRepository contestRepository)
        {
            _beerRepository = beerRepository;
            _contestRepository = contestRepository;
        }

        public async Task<string> Handle(RegisterBeerCommand request, CancellationToken cancellationToken)
        {
            // Check if the contest exists and is open for registration
            var contest = await _contestRepository.GetByIdAsync(request.ContestId);
            if (contest == null)
            {
                throw new Exception($"Contest with ID {request.ContestId} not found");
            }

            if (contest.Status != ContestStatus.RegistrationOpen)
            {
                throw new Exception("Contest is not open for registration");
            }

            // Check if the brewer has already registered the maximum number of beers
            int beerCount = await _beerRepository.GetBrewerBeerCountAsync(request.BrewerId, request.ContestId);
            if (beerCount >= contest.MaxBeersPerParticipant)
            {
                throw new Exception($"You have already registered the maximum number of beers ({contest.MaxBeersPerParticipant}) for this contest");
            }

            // Create the beer
            var beer = new Beer
            {
                Name = request.Name,
                Style = request.Style,
                Description = request.Description,
                AlcoholByVolume = request.AlcoholByVolume,
                Color = request.Color,
                Aroma = request.Aroma,
                Flavor = request.Flavor,
                Ingredients = request.Ingredients,
                BrewingProcess = request.BrewingProcess,
                BrewerId = request.BrewerId,
                BrewerName = request.BrewerName,
                BrewerEmail = request.BrewerEmail,
                BrewerPhone = request.BrewerPhone,
                ContestId = request.ContestId,
                RegistrationDate = DateTime.UtcNow
            };

            return await _beerRepository.CreateAsync(beer);
        }
    }

    public class RegisterBeerCommandValidator : IValidator<RegisterBeerCommand>
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IContestRepository _contestRepository;

        public RegisterBeerCommandValidator(
            IBeerRepository beerRepository,
            IContestRepository contestRepository)
        {
            _beerRepository = beerRepository;
            _contestRepository = contestRepository;
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterBeerCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                result.Errors.Add(new ValidationFailure("Name", "Beer name is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Style))
            {
                result.Errors.Add(new ValidationFailure("Style", "Beer style is required"));
            }

            if (command.AlcoholByVolume < 0 || command.AlcoholByVolume > 100)
            {
                result.Errors.Add(new ValidationFailure("AlcoholByVolume", "Alcohol by volume must be between 0 and 100"));
            }

            if (string.IsNullOrWhiteSpace(command.BrewerId))
            {
                result.Errors.Add(new ValidationFailure("BrewerId", "Brewer ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.BrewerName))
            {
                result.Errors.Add(new ValidationFailure("BrewerName", "Brewer name is required"));
            }

            if (string.IsNullOrWhiteSpace(command.BrewerEmail))
            {
                result.Errors.Add(new ValidationFailure("BrewerEmail", "Brewer email is required"));
            }

            if (string.IsNullOrWhiteSpace(command.ContestId))
            {
                result.Errors.Add(new ValidationFailure("ContestId", "Contest ID is required"));
            }
            else
            {
                // Check if the contest exists and is open for registration
                var contest = await _contestRepository.GetByIdAsync(command.ContestId);
                if (contest == null)
                {
                    result.Errors.Add(new ValidationFailure("ContestId", $"Contest with ID {command.ContestId} not found"));
                }
                else if (contest.Status != ContestStatus.RegistrationOpen)
                {
                    result.Errors.Add(new ValidationFailure("ContestId", "Contest is not open for registration"));
                }
                else
                {
                    // Check if the brewer has already registered the maximum number of beers
                    int beerCount = await _beerRepository.GetBrewerBeerCountAsync(command.BrewerId, command.ContestId);
                    if (beerCount >= contest.MaxBeersPerParticipant)
                    {
                        result.Errors.Add(new ValidationFailure("BrewerId", 
                            $"You have already registered the maximum number of beers ({contest.MaxBeersPerParticipant}) for this contest"));
                    }
                }
            }

            return result;
        }
    }
}