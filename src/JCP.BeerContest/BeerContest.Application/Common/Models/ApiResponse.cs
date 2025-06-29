namespace BeerContest.Application.Common.Models
{
    /// <summary>
    /// Standard response wrapper for API operations
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Succeeded { get; set; }
        
        /// <summary>
        /// Message describing the result of the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Collection of error messages when the operation fails
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Default constructor
        /// </summary>
        protected ApiResponse() { }
        
        /// <summary>
        /// Creates a success response with a message
        /// </summary>
        public static ApiResponse Success(string message = "")
        {
            return new ApiResponse { Succeeded = true, Message = message };
        }
        
        /// <summary>
        /// Creates a failure response with a message and optional errors
        /// </summary>
        public static ApiResponse Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
    
    /// <summary>
    /// Generic response wrapper that includes a data payload
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Data payload returned by the operation
        /// </summary>
        public T? Data { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        protected ApiResponse() { }
        
        /// <summary>
        /// Creates a success response with data and optional message
        /// </summary>
        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }
        
        /// <summary>
        /// Creates a failure response with a message and optional errors
        /// </summary>
        public static new ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}