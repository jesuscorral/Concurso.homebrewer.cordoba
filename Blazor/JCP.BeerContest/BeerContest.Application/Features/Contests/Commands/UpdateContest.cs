using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Commands
{
    public class UpdateContestCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Edition { get; set; }
        public string Description { get; set; }
        public string OrganizerEmail { get; set; }
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

    public class UpdateContestCommandHandler : IRequestHandler<UpdateContestCommand, bool>
    {
        private readonly IContestRepository _contestRepository;

        public UpdateContestCommandHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<bool> Handle(UpdateContestCommand request, CancellationToken cancellationToken)
        {
            // Check if the contest exists
            var existingContest = await _contestRepository.GetByIdAsync(request.Id);
            if (existingContest == null)
            {
                return false;
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
                MaxBeersPerParticipant = request.MaxBeersPerParticipant
            };

            // Save the updated contest to the repository
            return await _contestRepository.UpdateAsync(contest);
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

            return result;
        }
    }
}