using BeerContest.Application.Common.Behaviors;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RegisterParticipant
{
    public class RegisterParticipantCommand : IRequest<string>
    {
        public string ACCEMemberNumber { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string EmailUser { get; set; }
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
                ACCEMemberNumber = request.ACCEMemberNumber,
                FullName = request.FullName,
                BirthDate = request.BirthDate,
                Phone = request.Phone,
                EmailUser = request.EmailUser,
            };

            var participantCreated = await _participantRepository.CreateAsync(participant);

            return participantCreated;
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