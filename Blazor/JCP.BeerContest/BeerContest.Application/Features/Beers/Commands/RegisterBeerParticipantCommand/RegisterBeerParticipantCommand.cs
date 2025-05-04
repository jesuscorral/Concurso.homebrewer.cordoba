using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.RegisterBeerParticipantCommand
{
    public class RegisterBeerParticipantCommand : IRequest<string>
    {
       public string BeerId { get; set; }
        public string ParticipantId { get; set; }
        public string EntryInstructions { get; set; }
    }

    public class RegisterBeerParticipantCommandHandler : IRequestHandler<RegisterBeerParticipantCommand, string>
    {
        private readonly IBeerParticipantRepository _beerParticipantRepository;

        public RegisterBeerParticipantCommandHandler(
            IBeerParticipantRepository beerParticipantRepository)
        {
            _beerParticipantRepository = beerParticipantRepository;
        }

        public async Task<string> Handle(RegisterBeerParticipantCommand request, CancellationToken cancellationToken)
        {
            var beerParticipant = new BeerParticipant
            {
                BeerId = request.BeerId,
                ParticipantId = request.ParticipantId,
                EntryInstructions = request.EntryInstructions,
            };

            var beerParticipantCreated = await _beerParticipantRepository.CreateAsync(beerParticipant);
            return beerParticipantCreated;
        }
    }
}
