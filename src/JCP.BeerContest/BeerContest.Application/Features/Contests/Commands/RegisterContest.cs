using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Commands
{
    public class RegisterContestCommand : IApiRequest<string>
    {
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
        public double Discount { get; set; } = 0.0; // Default discount is 0%
        public List<ContestRule> Rules { get; set; } = new List<ContestRule>();
        public List<BeerCategory> Categories { get; set; } = new List<BeerCategory>();
        public ContestStatus Status { get; set; }
        public int MaxBeersPerParticipant { get; set; } = 3; // Default limit of 3 beers per participant // TODO: Remove magic numbers
    }

    public class RegisterContestCommandHandler : IApiRequestHandler<RegisterContestCommand, string>
    {
        private readonly IContestRepository _contestRepository;

        public RegisterContestCommandHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<ApiResponse<string>> Handle(RegisterContestCommand request, CancellationToken cancellationToken)
        {
            // Create the contest
            var contest = new Contest
            {
                Id = Guid.NewGuid().ToString(), // Generate a new unique ID for the contest
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

            // Save the contest to the repository
            var contestCreated = await _contestRepository.CreateAsync(contest);

            return ApiResponse<string>.Success(
                    contestCreated,
                    $"Successfully updated");
        }
    }

    public class RegisterContestCommandValidator : IValidator<RegisterContestCommand>
    {

        public RegisterContestCommandValidator()
        {
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterContestCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();
            return result;
        }

    }
}
