using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Users.Commands.RegisterGoogleUser
{
    public class RegisterGoogleUserCommand : IApiRequest<string>
    {
        public required string Id { get; set; } // This is the unique identifier for the user in your system
        public required string GoogleId { get; set; }
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public List<UserRole> Roles { get; set; } = new List<UserRole> { UserRole.Participant }; // Default role is Participant
    }

    public class RegisterGoogleUserCommandHandler : IApiRequestHandler<RegisterGoogleUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public RegisterGoogleUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<string>> Handle(RegisterGoogleUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user already exists
                User? existingUser = null;
                
                try
                {
                    existingUser = await _userRepository.GetByEmailAsync(request.Email);
                }
                catch (KeyNotFoundException)
                {
                    // User doesn't exist, that's expected in some cases
                }
                
                if (existingUser != null)
                {
                    // Update the existing user with Google ID if not already set
                    if (string.IsNullOrEmpty(existingUser.GoogleId))
                    {
                        existingUser.GoogleId = request.GoogleId;
                        await _userRepository.UpdateAsync(existingUser);
                    }

                    // Update last login time
                    existingUser.LastLoginAt = DateTime.UtcNow;
                    await _userRepository.UpdateAsync(existingUser);

                    return ApiResponse<string>.Success(
                        existingUser.Id, 
                        "User login successful"
                    );
                }

                // Create a new user
                var user = new User
                {
                    Id = request.Id, // Use the provided ID or generate a new one if necessary
                    GoogleId = request.GoogleId,
                    Email = request.Email,
                    DisplayName = request.DisplayName,
                    Roles = request.Roles, 
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow
                };

                var userId = await _userRepository.CreateAsync(user);
                return ApiResponse<string>.Success(userId, "User registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(
                    "Failed to register user",
                    new List<string> { ex.Message }
                );
            }
        }
    }

    public class RegisterGoogleUserCommandValidator : IValidator<RegisterGoogleUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(ValidationContext<RegisterGoogleUserCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.GoogleId))
            {
                result.Errors.Add(new ValidationFailure("GoogleId", "Google ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", "Email is required"));
            }
            else if (!IsValidEmail(command.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", "Email is not valid"));
            }

            if (string.IsNullOrWhiteSpace(command.DisplayName))
            {
                result.Errors.Add(new ValidationFailure("DisplayName", "Display name is required"));
            }

            return Task.FromResult(result);
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