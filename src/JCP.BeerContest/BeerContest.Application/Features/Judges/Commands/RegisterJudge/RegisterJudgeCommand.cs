using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Judges.Commands.RegisterJudge
{
    public class RegisterJudgeCommand : IApiRequest<string>
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Preferences { get; set; }
        public required string BcjpId { get; set; }
        public required string ContestId { get; set; }
        public required string ContestName { get; set; }
    }
 
    public class RegisterJudgeCommandHandler : IApiRequestHandler<RegisterJudgeCommand, string>
    {
        private readonly IJudgeRepository _judgeRepository;

        public RegisterJudgeCommandHandler(IJudgeRepository judgeRepository)
        {
            _judgeRepository = judgeRepository;
        }

        public async Task<ApiResponse<string>> Handle(RegisterJudgeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return ApiResponse<string>.Failure("Email is required");
                }

                // Since there's no GetByEmailAsync method, we'll check for duplicates using the existing methods
                var allJudgesInContest = await _judgeRepository.GetAllByContestAsync(request.ContestId);
                var existingJudge = allJudgesInContest.FirstOrDefault(j => j.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
                
                if (existingJudge != null)
                {
                    return ApiResponse<string>.Failure("A judge with this email already exists in this contest");
                }

                var judge = new Judge
                {
                    Id = Guid.NewGuid().ToString(), // Generate a new ID for the judge
                    Name = request.Name,
                    Surname = request.Surname,
                    Phone = request.Phone,
                    Email = request.Email,
                    Preferences = request.Preferences,
                    BcjpId = request.BcjpId,
                    ContestId = request.ContestId,
                    ContestName = request.ContestName,
                    CreatedAt = DateTime.UtcNow
                };

                var judgeId = await _judgeRepository.CreateAsync(judge);
                return ApiResponse<string>.Success(judgeId, "Judge registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(
                    "Failed to register judge", 
                    new List<string> { ex.Message });
            }
        }
    }

    public class RegisterJudgeCommandValidator : BeerContest.Application.Common.Behaviors.IValidator<RegisterJudgeCommand>
    {
        public async Task<BeerContest.Application.Common.Behaviors.ValidationResult> ValidateAsync(
            BeerContest.Application.Common.Behaviors.ValidationContext<RegisterJudgeCommand> context, 
            CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new BeerContest.Application.Common.Behaviors.ValidationResult();

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                result.Errors.Add(new ValidationFailure("Name", "Name is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Surname))
            {
                result.Errors.Add(new ValidationFailure("Surname", "Surname is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", "Email is required"));
            }
            else if (!IsValidEmail(command.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", "Email is not valid"));
            }

            if (string.IsNullOrWhiteSpace(command.Phone))
            {
                result.Errors.Add(new ValidationFailure("Phone", "Phone number is required"));
            }

            if (string.IsNullOrWhiteSpace(command.ContestId))
            {
                result.Errors.Add(new ValidationFailure("ContestId", "Contest ID is required"));
            }

            return result;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}