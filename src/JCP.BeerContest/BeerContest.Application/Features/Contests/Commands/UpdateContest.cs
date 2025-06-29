using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Contests.Commands
{
    public class UpdateContestCommand : IApiRequest<bool>
    {
        public required string Id { get; set; }
        public required string Edition { get; set; }
        public required string Description { get; set; }
        public required string OrganizerEmail { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime ShipphingStartDate { get; set; }
        public DateTime ShipphingEndDate { get; set; }
        public double EntryFee1Beer { get; set; }
        public double EntryFee2Beer { get; set; }
        public double EntryFee3Beer { get; set; }
        public double Discount { get; set; } = 0.0;
        public List<ContestRule> Rules { get; set; } = new List<ContestRule>();
        public List<BeerCategory> Categories { get; set; } = new List<BeerCategory>();
        public ContestStatus Status { get; set; }
        public int MaxBeersPerParticipant { get; set; } = 3;
    }

    public class UpdateContestCommandHandler : IApiRequestHandler<UpdateContestCommand, bool>
    {
        private readonly IContestRepository _contestRepository;

        public UpdateContestCommandHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateContestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if the contest exists
                var existingContest = await _contestRepository.GetByIdAsync(request.Id);
                if (existingContest == null)
                {
                    return ApiResponse<bool>.Failure($"Contest with ID {request.Id} not found");
                }

                // Update the contest
                var contest = new Contest
                {
                    Id = request.Id,
                    Edition = request.Edition,
                    Description = request.Description,
                    OrganizerEmail = request.OrganizerEmail,
                    RegistrationStartDate = request.RegistrationStartDate,
                    RegistrationEndDate = request.RegistrationEndDate,
                    ShipphingStartDate = request.ShipphingStartDate,
                    ShipphingEndDate = request.ShipphingEndDate,
                    EntryFee1Beer = request.EntryFee1Beer,
                    EntryFee2Beer = request.EntryFee2Beer,
                    EntryFee3Beer = request.EntryFee3Beer,
                    Discount = request.Discount,
                    Rules = request.Rules,
                    Categories = request.Categories,
                    Status = request.Status,
                    MaxBeersPerParticipant = request.MaxBeersPerParticipant,
                    CreatedAt = existingContest.CreatedAt
                };

                // Save the updated contest to the repository
                await _contestRepository.UpdateAsync(contest);
                return ApiResponse<bool>.Success(true, "Contest updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Failed to update contest",
                    new List<string> { ex.Message }
                );
            }
        }
    }

    public class UpdateContestCommandValidator : IValidator<UpdateContestCommand>
    {
        public UpdateContestCommandValidator()
        {
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateContestCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            // Add validation rules as needed
            if (string.IsNullOrWhiteSpace(command.Id))
            {
                result.Errors.Add(new ValidationFailure("Id", "Contest ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Edition))
            {
                result.Errors.Add(new ValidationFailure("Edition", "Edition is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Description))
            {
                result.Errors.Add(new ValidationFailure("Description", "Description is required"));
            }

            if (string.IsNullOrWhiteSpace(command.OrganizerEmail))
            {
                result.Errors.Add(new ValidationFailure("OrganizerEmail", "Organizer email is required"));
            }

            if (command.RegistrationEndDate <= command.RegistrationStartDate)
            {
                result.Errors.Add(new ValidationFailure("RegistrationEndDate", 
                    "Registration end date must be after registration start date"));
            }

            if (command.ShipphingEndDate <= command.ShipphingStartDate)
            {
                result.Errors.Add(new ValidationFailure("ShipphingEndDate", 
                    "Shipping end date must be after shipping start date"));
            }

            return result;
        }
    }
}