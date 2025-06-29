using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Beers.Commands.RegisterBeer
{
    public class RegisterBeerCommand : IApiRequest<string>
    {
        // Beer information
        public BeerCategory Category { get; set; }
        public required string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public required string Malts { get; set; }
        public required string Hops { get; set; }
        public required string Yeast { get; set; }
        public required string Additives { get; set; }
        public required string ParticipantId { get; set; }
        public required string ParticipantEmail { get; set; }
        public required string EntryInstructions { get; set; }
        public required string ContestId { get; set; }
    }

    public class RegisterBeerCommandHandler : IApiRequestHandler<RegisterBeerCommand, string>
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

        public async Task<ApiResponse<string>> Handle(RegisterBeerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if the contest exists and is open for registration
                var contest = await _contestRepository.GetByIdAsync(request.ContestId);
                if (contest == null)
                {
                    return ApiResponse<string>.Failure($"Contest with ID {request.ContestId} not found");
                }
                
                if (contest.Status != ContestStatus.RegistrationOpen)
                {
                    return ApiResponse<string>.Failure("Contest is not open for registration");
                }
                
                // Check if the participant has already registered the maximum number of beers
                var participantBeers = await _beerRepository.GetByParticipantAndContestIdAsync(request.ParticipantEmail, request.ContestId);
                var beerCount = participantBeers?.Count() ?? 0;
                
                if (beerCount >= contest.MaxBeersPerParticipant)
                {
                    return ApiResponse<string>.Failure(
                        $"You have already registered the maximum number of beers ({contest.MaxBeersPerParticipant}) for this contest");
                }

                // Create the beer
                var beer = new Beer
                {
                    Id = Guid.NewGuid().ToString(),
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

                var beerId = await _beerRepository.CreateAsync(beer);
                return ApiResponse<string>.Success(beerId, "Beer registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure("Failed to register beer", new List<string> { ex.Message });
            }
        }
    }

    public class RegisterBeerCommandValidator : IValidator<RegisterBeerCommand>
    {
        public RegisterBeerCommandValidator() { }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterBeerCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.BeerStyle))
            {
                result.Errors.Add(new ValidationFailure("BeerStyle", "Beer style is required"));
            }

            if (command.AlcoholContent < 0 || command.AlcoholContent > 100)
            {
                result.Errors.Add(new ValidationFailure("AlcoholContent", "Alcohol content must be between 0 and 100"));
            }

            if (string.IsNullOrWhiteSpace(command.ParticipantId))
            {
                result.Errors.Add(new ValidationFailure("ParticipantId", "Participant ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.ParticipantEmail))
            {
                result.Errors.Add(new ValidationFailure("ParticipantEmail", "Participant email is required"));
            }

            if (string.IsNullOrWhiteSpace(command.ContestId))
            {
                result.Errors.Add(new ValidationFailure("ContestId", "Contest ID is required"));
            }
            
            if (command.BottleDate > DateTime.Now)
            {
                result.Errors.Add(new ValidationFailure("BottleDate", "Bottle date cannot be in the future"));
            }
            
            if (command.ElaborationDate > DateTime.Now)
            {
                result.Errors.Add(new ValidationFailure("ElaborationDate", "Elaboration date cannot be in the future"));
            }
            
            if (command.ElaborationDate > command.BottleDate)
            {
                result.Errors.Add(new ValidationFailure("ElaborationDate", "Elaboration date must be before bottle date"));
            }

            return result;
        }
    }
}