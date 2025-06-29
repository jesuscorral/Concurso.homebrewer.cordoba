using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Repositories;

namespace BeerContest.Application.Features.Beers.Commands.DeleteBeer
{
    public class DeleteBeerCommand : IApiRequest<bool>
    {
        public required string BeerId { get; set; }
    }

    public class DeleteBeerCommandHandler : IApiRequestHandler<DeleteBeerCommand, bool>
    {
        private readonly IBeerRepository _beerRepository;

        public DeleteBeerCommandHandler(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if beer exists first
                var beer = await _beerRepository.GetByIdAsync(request.BeerId);
                if (beer == null)
                {
                    return ApiResponse<bool>.Failure($"Beer with ID {request.BeerId} not found");
                }

                await _beerRepository.DeleteAsync(request.BeerId);
                return ApiResponse<bool>.Success(true, "Beer deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("Failed to delete beer", new List<string> { ex.Message });
            }
        }
    }
}