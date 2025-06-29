using BeerContest.Application.Common.Interfaces;
using BeerContest.Application.Common.Models;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeerContest.Application.Features.Judges.Queries.GetAllJudgesByContest
{
    public class GetAllJudgesByContestCommand : IApiRequest<IEnumerable<Judge>>
    {
        public required string ContestId { get; set; }
    }

    public class GetAllJudgesCommandHandler : IApiRequestHandler<GetAllJudgesByContestCommand, IEnumerable<Judge>>
    {
        private readonly IJudgeRepository _judgeRepository;
        
        public GetAllJudgesCommandHandler(IJudgeRepository judgeRepository)
        {
            _judgeRepository = judgeRepository;
        }
        
        public async Task<ApiResponse<IEnumerable<Judge>>> Handle(GetAllJudgesByContestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContestId))
                {
                    return ApiResponse<IEnumerable<Judge>>.Failure("Contest ID is required");
                }

                var judges = await _judgeRepository.GetAllByContestAsync(request.ContestId);
                
                if (judges == null || !judges.Any())
                {
                    return ApiResponse<IEnumerable<Judge>>.Success(
                        new List<Judge>(), 
                        "No judges found for this contest");
                }
                
                return ApiResponse<IEnumerable<Judge>>.Success(
                    judges, 
                    $"Successfully retrieved {judges.Count()} judges");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Judge>>.Failure(
                    "Failed to retrieve judges",
                    new List<string> { ex.Message });
            }
        }
    }
}
