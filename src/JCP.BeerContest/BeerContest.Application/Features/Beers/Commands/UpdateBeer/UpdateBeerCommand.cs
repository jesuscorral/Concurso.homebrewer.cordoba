using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Beers.Commands.UpdateBeer
{
    public class UpdateBeerCommand : IApiRequest<string>
    {
        public required string Id { get; set; }
        public BeerCategory Category { get; set; }
        public required string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public required string Malts { get; set; }
        public required string Hops { get; set; }
        public required string Yeast { get; set; }
        public required string Additives { get; set; }
        public required string ParticipantEmail { get; set; }
        public required string EntryInstructions { get; set; }
    }

    public class UpdateBeerCommandHandler : IApiRequestHandler<UpdateBeerCommand, string>
    {
        private readonly IBeerRepository _beerRepository;

        public UpdateBeerCommandHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<ApiResponse<string>> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the existing beer
                var existingBeer = await _beerRepository.GetByIdAsync(request.Id);
                
                if (existingBeer == null)
                {
                    return ApiResponse<string>.Failure($"Beer with ID {request.Id} not found");
                }
                
                // Check if the user is authorized to update this beer
                if (existingBeer.ParticipantEmail != request.ParticipantEmail)
                {
                    return ApiResponse<string>.Failure("You are not authorized to update this beer");
                }
                
                // Update the beer properties
                existingBeer.Category = request.Category;
                existingBeer.BeerStyle = request.BeerStyle;
                existingBeer.AlcoholContent = request.AlcoholContent;
                existingBeer.ElaborationDate = request.ElaborationDate;
                existingBeer.BottleDate = request.BottleDate;
                existingBeer.Malts = request.Malts;
                existingBeer.Hops = request.Hops;
                existingBeer.Yeast = request.Yeast;
                existingBeer.Additives = request.Additives;
                existingBeer.EntryInstructions = request.EntryInstructions;

                // Update the beer in the repository
                await _beerRepository.UpdateAsync(existingBeer);
                
                return ApiResponse<string>.Success(existingBeer.Id, "Beer updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure("Failed to update beer", new List<string> { ex.Message });
            }
        }
    }

    public class UpdateBeerCommandValidator : IValidator<UpdateBeerCommand>
    {
        public UpdateBeerCommandValidator() { }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateBeerCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.Id))
            {
                result.Errors.Add(new ValidationFailure("Id", "Beer ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.BeerStyle))
            {
                result.Errors.Add(new ValidationFailure("BeerStyle", "Beer style is required"));
            }

            if (command.AlcoholContent < 0 || command.AlcoholContent > 100)
            {
                result.Errors.Add(new ValidationFailure("AlcoholContent", "Alcohol content must be between 0 and 100"));
            }

            if (string.IsNullOrWhiteSpace(command.Malts))
            {
                result.Errors.Add(new ValidationFailure("Malts", "Malts are required"));
            }

            if (string.IsNullOrWhiteSpace(command.Hops))
            {
                result.Errors.Add(new ValidationFailure("Hops", "Hops are required"));
            }

            if (string.IsNullOrWhiteSpace(command.Yeast))
            {
                result.Errors.Add(new ValidationFailure("Yeast", "Yeast is required"));
            }

            if (string.IsNullOrWhiteSpace(command.ParticipantEmail))
            {
                result.Errors.Add(new ValidationFailure("ParticipantEmail", "Participant email is required"));
            }

            return result;
        }
    }
}
