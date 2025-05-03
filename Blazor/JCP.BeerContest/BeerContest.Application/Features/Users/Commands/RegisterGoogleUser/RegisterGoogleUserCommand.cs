using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Users.Commands.RegisterGoogleUser
{
    public class RegisterGoogleUserCommand : IRequest<string>
    {
        public string Id { get; set; } // This is the unique identifier for the user in your system
        public string GoogleId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public List<UserRole> Roles { get; set; } = new List<UserRole> { UserRole.Participant }; // Default role is Participant
    }

    public class RegisterGoogleUserCommandHandler : IRequestHandler<RegisterGoogleUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public RegisterGoogleUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(RegisterGoogleUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
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

                //return existingUser.GoogleId;
                return existingUser.Id;
            }

            // Create a new user
            var user = new User
            {
                GoogleId = request.GoogleId,
                Email = request.Email,
                DisplayName = request.DisplayName,
                Roles = request.Roles, 
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            return await _userRepository.CreateAsync(user);
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