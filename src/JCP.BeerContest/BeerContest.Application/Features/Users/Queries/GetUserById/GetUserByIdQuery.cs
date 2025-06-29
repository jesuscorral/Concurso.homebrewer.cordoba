using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IApiRequest<User>
    {
        public required string Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IApiRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return ApiResponse<User>.Failure("User ID is required");
                }

                var user = await _userRepository.GetByIdAsync(request.Id);
                
                if (user == null)
                {
                    return ApiResponse<User>.Failure($"User with ID {request.Id} not found");
                }

                return ApiResponse<User>.Success(user, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.Failure(
                    "Failed to retrieve user", 
                    new List<string> { ex.Message });
            }
        }
    }
}