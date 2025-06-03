using BeerContest.Domain.Repositories;
using MediatR;

namespace BeerContest.Application.Features.Beers.Commands.DeleteBeer
{
    public class DeleteBeerCommand : IRequest<bool>
    {
        public string BeerId { get; set; }
    }

    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, bool>
    {
        private readonly IBeerRepository _beerRepository;

        public DeleteBeerCommandHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<bool> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            await _beerRepository.DeleteAsync(request.BeerId);
            return true;
        }
    }
}