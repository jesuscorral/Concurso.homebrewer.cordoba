namespace BeerContest.Infrastructure.Common.Abstractions
{
    /// <summary>
    /// Unit of Work pattern for managing transactions across multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Begins a new transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Executes multiple operations in a single transaction
        /// </summary>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);

        /// <summary>
        /// Executes multiple operations in a single transaction without return value
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> operation);

        /// <summary>
        /// Creates a batch operation for multiple writes
        /// </summary>
        Task<IBatchOperation> CreateBatchAsync();
    }

    /// <summary>
    /// Batch operation interface for bulk operations
    /// </summary>
    public interface IBatchOperation : IDisposable
    {
        /// <summary>
        /// Adds an entity to the batch
        /// </summary>
        void Add<T>(string collectionName, T entity) where T : class;

        /// <summary>
        /// Updates an entity in the batch
        /// </summary>
        void Update<T>(string collectionName, string documentId, T entity) where T : class;

        /// <summary>
        /// Updates specific fields in the batch
        /// </summary>
        void UpdateFields(string collectionName, string documentId, Dictionary<string, object> updates);

        /// <summary>
        /// Deletes an entity in the batch
        /// </summary>
        void Delete(string collectionName, string documentId);

        /// <summary>
        /// Commits all batch operations
        /// </summary>
        Task CommitAsync();
    }
}
