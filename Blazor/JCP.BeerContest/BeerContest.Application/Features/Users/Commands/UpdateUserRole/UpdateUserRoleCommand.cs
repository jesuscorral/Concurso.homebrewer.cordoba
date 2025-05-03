using System;
using System.Threading;
using System.Threading.Tasks;
using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Users.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public UserRole NewRole { get; set; }
        public string AdminId { get; set; } // ID of the administrator making the change
        public bool RefreshClaims { get; set; } = true; // Whether to refresh the user's claims
    }

    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserRoleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            // Verify that the admin user exists and is an administrator
            var adminUser = await _userRepository.GetByIdAsync(request.AdminId);
            if (adminUser == null)
            {
                throw new Exception($"Admin user with ID {request.AdminId} not found");
            }

            if (!adminUser.Roles.Contains(UserRole.Administrator))
            {
                throw new Exception($"User with ID {request.AdminId} is not an administrator");
            }

            // Get the user to update
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception($"User with ID {request.UserId} not found");
            }

            // Update the user's role
            user.Roles = new List<UserRole> { request.NewRole };
            await _userRepository.UpdateAsync(user);
            
            return true;
        }
    }

    public class UpdateUserRoleCommandValidator : IValidator<UpdateUserRoleCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserRoleCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateUserRoleCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.UserId))
            {
                result.Errors.Add(new ValidationFailure("UserId", "User ID is required"));
            }

            if (string.IsNullOrWhiteSpace(command.AdminId))
            {
                result.Errors.Add(new ValidationFailure("AdminId", "Admin ID is required"));
            }
            else
            {
                // Verify that the admin user exists and is an administrator
                var adminUser = await _userRepository.GetByIdAsync(command.AdminId);
                if (adminUser == null)
                {
                    result.Errors.Add(new ValidationFailure("AdminId", $"Admin user with ID {command.AdminId} not found"));
                }
                else if (!adminUser.Roles.Contains(UserRole.Administrator))
                {
                    result.Errors.Add(new ValidationFailure("AdminId", $"User with ID {command.AdminId} is not an administrator"));
                }
            }

            // Verify that the user to update exists
            if (!string.IsNullOrWhiteSpace(command.UserId))
            {
                var user = await _userRepository.GetByIdAsync(command.UserId);
                if (user == null)
                {
                    result.Errors.Add(new ValidationFailure("UserId", $"User with ID {command.UserId} not found"));
                }
            }

            return result;
        }
    }
}