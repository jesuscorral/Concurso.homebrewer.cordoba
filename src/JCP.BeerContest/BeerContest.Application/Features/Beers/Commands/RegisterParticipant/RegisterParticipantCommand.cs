using BeerContest.Application.Common.Behaviors;
using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Commands.RegisterParticipant
{
    public class RegisterParticipantCommand : IApiRequest<string>
    {
        public required string ACCEMemberNumber { get; set; }
        public required string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Phone { get; set; }
        public required string EmailUser { get; set; }
    }

    public class RegisterParticipantCommandHandler : IApiRequestHandler<RegisterParticipantCommand, string>
    {
        private readonly IParticipantRepository _participantRepository;

        public RegisterParticipantCommandHandler(
            IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<ApiResponse<string>> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.EmailUser))
                {
                    return ApiResponse<string>.Failure("Email is required");
                }

                var participant = new Participant
                {
                    Id = Guid.NewGuid().ToString(), // Generate a new unique ID for the participant
                    ACCEMemberNumber = request.ACCEMemberNumber,
                    FullName = request.FullName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    EmailUser = request.EmailUser,
                };

                var existsParticipant = await _participantRepository.GetByEmailUserAsync(participant.EmailUser);
                if (existsParticipant == null)
                {
                    var participantCreated = await _participantRepository.CreateAsync(participant);
                    return ApiResponse<string>.Success(participantCreated, "Participant created");

                }
                else
                {
                    return ApiResponse<string>.Success(existsParticipant.Id, "Participant already exists");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure("Failed to create participant", new List<string> { ex.Message });
            }


        }
    }

    public class RegisterParticipantCommandValidator : IValidator<RegisterParticipantCommand>
    {

        public RegisterParticipantCommandValidator()
        {
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterParticipantCommand> context, CancellationToken cancellationToken)
        {
            var command = context.Instance;
            var result = new ValidationResult();


            return result;
        }
    }
}