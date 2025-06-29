using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RegisterParticipant
{
    public class RegisterParticipantCommand : IRequest<string>
    {
        public required string ACCEMemberNumber { get; set; }
        public required string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Phone { get; set; }
        public required string EmailUser { get; set; }
    }

    public class RegisterParticipantCommandHandler : IRequestHandler<RegisterParticipantCommand, string>
    {
        private readonly IParticipantRepository _participantRepository;

        public RegisterParticipantCommandHandler(
            IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<string> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
        {
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
                return participantCreated;
            }
            else
            {
                return existsParticipant.Id;
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