using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Contests.Commands.CreateContest
{
    public class CreateContestCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string OrganizerId { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public List<ContestRuleDto> Rules { get; set; } = new List<ContestRuleDto>();
        public List<BeerCategoryDto> Categories { get; set; } = new List<BeerCategoryDto>();
        public int MaxBeersPerParticipant { get; set; } = 3;
    }

    public class ContestRuleDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class BeerCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateContestCommandHandler : IRequestHandler<CreateContestCommand, string>
    {
        private readonly IContestRepository _contestRepository;

        public CreateContestCommandHandler(IContestRepository contestRepository)
        {
            _contestRepository = contestRepository;
        }

        public async Task<string> Handle(CreateContestCommand request, CancellationToken cancellationToken)
        {
            var contest = new Contest
            {
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                OrganizerId = request.OrganizerId,
                RegistrationStartDate = request.RegistrationStartDate,
                RegistrationEndDate = request.RegistrationEndDate,
                Status = ContestStatus.Draft,
                MaxBeersPerParticipant = request.MaxBeersPerParticipant
            };

            // Map rules
            if (request.Rules != null && request.Rules.Count > 0)
            {
                foreach (var ruleDto in request.Rules)
                {
                    contest.Rules.Add(new ContestRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = ruleDto.Title,
                        Description = ruleDto.Description,
                        Order = ruleDto.Order
                    });
                }
            }

            // Map categories
            if (request.Categories != null && request.Categories.Count > 0)
            {
                foreach (var categoryDto in request.Categories)
                {
                    contest.Categories.Add(new BeerCategory2
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = categoryDto.Name,
                        Description = categoryDto.Description
                    });
                }
            }

            return await _contestRepository.CreateAsync(contest);
        }
    }

    public class CreateContestCommandValidator : IValidator<CreateContestCommand>
    {
        public Task<ValidationResult> ValidateAsync(ValidationContext<CreateContestCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                result.Errors.Add(new ValidationFailure("Name", "Name is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Location))
            {
                result.Errors.Add(new ValidationFailure("Location", "Location is required"));
            }

            if (command.Date < DateTime.Now)
            {
                result.Errors.Add(new ValidationFailure("Date", "Contest date must be in the future"));
            }

            if (command.RegistrationStartDate >= command.RegistrationEndDate)
            {
                result.Errors.Add(new ValidationFailure("RegistrationStartDate", "Registration start date must be before end date"));
            }

            if (command.RegistrationEndDate >= command.Date)
            {
                result.Errors.Add(new ValidationFailure("RegistrationEndDate", "Registration end date must be before contest date"));
            }

            if (command.MaxBeersPerParticipant <= 0)
            {
                result.Errors.Add(new ValidationFailure("MaxBeersPerParticipant", "Maximum beers per participant must be greater than zero"));
            }

            return Task.FromResult(result);
        }
    }
}