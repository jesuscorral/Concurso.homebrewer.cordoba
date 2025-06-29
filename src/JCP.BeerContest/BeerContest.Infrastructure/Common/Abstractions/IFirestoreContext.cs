using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Common.Abstractions
{
    /// <summary>
    /// Firestore-specific context interface for database operations
    /// </summary>
    public interface IFirestoreContext
    {
        /// <summary>
        /// Gets the Firestore database instance
        /// </summary>
        Task<FirestoreDb> GetDatabaseAsync();

        /// <summary>
        /// Gets a collection reference by name
        /// </summary>
        Task<CollectionReference> GetCollectionAsync(string collectionName);

        /// <summary>
        /// Executes a transaction
        /// </summary>
        Task<T> RunTransactionAsync<T>(Func<Transaction, Task<T>> operation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a transaction without return value
        /// </summary>
        Task RunTransactionAsync(Func<Transaction, Task> operation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a batch operation
        /// </summary>
        Task<WriteBatch> CreateBatchAsync();

        /// <summary>
        /// Commits a batch operation
        /// </summary>
        Task CommitBatchAsync(WriteBatch batch, CancellationToken cancellationToken = default);
    }
}
