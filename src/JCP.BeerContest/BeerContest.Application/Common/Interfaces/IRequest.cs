using BeerContest.Application.Common.Models;
using MediatR;

namespace BeerContest.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for requests that return a standardized response with no data
    /// </summary>
    public interface IApiRequest : IRequest<ApiResponse>
    {
    }

    /// <summary>
    /// Interface for requests that return a standardized response with data
    /// </summary>
    public interface IApiRequest<T> : IRequest<ApiResponse<T>>
    {
    }

    /// <summary>
    /// Interface for handlers that process requests returning a standardized response with no data
    /// </summary>
    public interface IApiRequestHandler<TRequest> : IRequestHandler<TRequest, ApiResponse>
        where TRequest : IApiRequest
    {
    }

    /// <summary>
    /// Interface for handlers that process requests returning a standardized response with data
    /// </summary>
    public interface IApiRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, ApiResponse<TResponse>>
        where TRequest : IApiRequest<TResponse>
    {
    }
}