using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RegisterBeer
{
    public class RegisterBeerCommand : IRequest<string>
    {
        // Beer information
        public BeerCategory Category { get; set; }
        public string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public string Malts { get; set; }
        public string Hops { get; set; }
        public string Yeast { get; set; }
        public string Additives { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantEmail { get; set; }
        public string EntryInstructions { get; set; }
        public string ContestId { get; set; }
    }

    public class RegisterBeerCommandHandler : IRequestHandler<RegisterBeerCommand, string>
    {
        private readonly IBeerRepository _beerRepository;

        public RegisterBeerCommandHandler(
            IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<string> Handle(RegisterBeerCommand request, CancellationToken cancellationToken)
        {
            // TODO: Check if the contest is open for registration
            // TODO: Añadir settings del concurso donde definir las fechas y demás requisitos

            //TODO: Check if the brewer has already registered the maximum number of beers
            //int beerCount = await _beerRepository.GetBrewerBeerCountAsync(request.BrewerId, request.ContestId);
            //if (beerCount >= contest.MaxBeersPerParticipant)
            //{
            //    throw new Exception($"You have already registered the maximum number of beers ({contest.MaxBeersPerParticipant}) for this contest");
            //}

            // Create the beer
            var beer = new Beer
            {
                Category = request.Category,
                BeerStyle = request.BeerStyle,
                AlcoholContent = request.AlcoholContent,
                ElaborationDate = request.ElaborationDate,
                BottleDate = request.BottleDate,
                Malts = request.Malts,
                Hops = request.Hops,
                Yeast = request.Yeast,
                Additives = request.Additives,
                ParticpantId = request.ParticipantId,
                ParticipantEmail = request.ParticipantEmail,
                EntryInstructions = request.EntryInstructions,
                ContestId = request.ContestId,
                CreatedAt = DateTime.UtcNow
            };

           
            var beerCreated = await _beerRepository.CreateAsync(beer);

            return beerCreated;
        }
    }

    public class RegisterBeerCommandValidator : IValidator<RegisterBeerCommand>
    {
        public RegisterBeerCommandValidator()
        {
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterBeerCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            //if (string.IsNullOrWhiteSpace(command.Name))
            //{
            //    result.Errors.Add(new ValidationFailure("Name", "Beer name is required"));
            //}

            //if (string.IsNullOrWhiteSpace(command.Style))
            //{
            //    result.Errors.Add(new ValidationFailure("Style", "Beer style is required"));
            //}

            //if (command.AlcoholByVolume < 0 || command.AlcoholByVolume > 100)
            //{
            //    result.Errors.Add(new ValidationFailure("AlcoholByVolume", "Alcohol by volume must be between 0 and 100"));
            //}

            //if (string.IsNullOrWhiteSpace(command.BrewerId))
            //{
            //    result.Errors.Add(new ValidationFailure("BrewerId", "Brewer ID is required"));
            //}

            //if (string.IsNullOrWhiteSpace(command.BrewerName))
            //{
            //    result.Errors.Add(new ValidationFailure("BrewerName", "Brewer name is required"));
            //}

            //if (string.IsNullOrWhiteSpace(command.BrewerEmail))
            //{
            //    result.Errors.Add(new ValidationFailure("BrewerEmail", "Brewer email is required"));
            //}

            //if (string.IsNullOrWhiteSpace(command.ContestId))
            //{
            //    result.Errors.Add(new ValidationFailure("ContestId", "Contest ID is required"));
            //}
            //else
            //{
            //    // Check if the contest exists and is open for registration
            //    var contest = await _contestRepository.GetByIdAsync(command.ContestId);
            //    if (contest == null)
            //    {
            //        result.Errors.Add(new ValidationFailure("ContestId", $"Contest with ID {command.ContestId} not found"));
            //    }
            //    else if (contest.Status != ContestStatus.RegistrationOpen)
            //    {
            //        result.Errors.Add(new ValidationFailure("ContestId", "Contest is not open for registration"));
            //    }
            //    else
            //    {
            //        // Check if the brewer has already registered the maximum number of beers
            //        int beerCount = await _beerRepository.GetBrewerBeerCountAsync(command.BrewerId, command.ContestId);
            //        if (beerCount >= contest.MaxBeersPerParticipant)
            //        {
            //            result.Errors.Add(new ValidationFailure("BrewerId", 
            //                $"You have already registered the maximum number of beers ({contest.MaxBeersPerParticipant}) for this contest"));
            //        }
            //    }
            //}

            return result;
        }
    }
}