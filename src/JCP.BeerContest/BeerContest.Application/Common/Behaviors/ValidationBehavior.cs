using BeerContest.Application.Common.Models;
using MediatR;

namespace BeerContest.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    // For ApiResponse wrapped types, return a failure response instead of throwing an exception
                    Type responseType = typeof(TResponse);
                    
                    if (typeof(ApiResponse).IsAssignableFrom(responseType))
                    {
                        return CreateValidationFailureResponse(failures);
                    }
                    
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }

        private TResponse CreateValidationFailureResponse(List<ValidationFailure> failures)
        {
            var errorMessages = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();
            Type responseType = typeof(TResponse);
            
            // If it's a generic ApiResponse<T>
            if (responseType.IsGenericType && 
                responseType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
            {
                var typeArg = responseType.GetGenericArguments()[0];
                var genericType = typeof(ApiResponse<>).MakeGenericType(typeArg);
                var failureMethod = genericType.GetMethod("Failure", new[] { typeof(string), typeof(List<string>) });
                
                if (failureMethod != null)
                {
                    var result = failureMethod.Invoke(null, new object[] { "Validation failed", errorMessages });
                    return (TResponse)result;
                }
            }
            
            // If it's a non-generic ApiResponse
            if (typeof(ApiResponse).IsAssignableFrom(responseType))
            {
                var result = ApiResponse.Failure("Validation failed", errorMessages);
                return (TResponse)(object)result;
            }
            
            throw new InvalidOperationException($"Cannot create validation failure response for type {responseType.Name}");
        }
    }

    // Simple validation interfaces
    public interface IValidator<T>
    {
        Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellationToken);
    }

    public class ValidationContext<T>
    {
        public T Instance { get; }

        public ValidationContext(T instance)
        {
            Instance = instance;
        }
    }

    public class ValidationResult
    {
        public List<ValidationFailure> Errors { get; }

        public ValidationResult()
        {
            Errors = new List<ValidationFailure>();
        }
    }

    public class ValidationFailure
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }

        public ValidationFailure(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }

    public class ValidationException : System.Exception
    {
        public List<ValidationFailure> Errors { get; }

        public ValidationException(List<ValidationFailure> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }
    }
}