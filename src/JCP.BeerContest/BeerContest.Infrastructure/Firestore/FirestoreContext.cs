using BeerContest.Infrastructure.Common.Abstractions;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Firestore
{
    /// <summary>
    /// Enhanced Firestore context with improved error handling and transaction support
    /// </summary>
    public class EnhancedFirestoreContext : IFirestoreContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EnhancedFirestoreContext> _logger;
        private FirestoreDb? _database;

        public EnhancedFirestoreContext(
            IConfiguration configuration,
            ILogger<EnhancedFirestoreContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Gets the Firestore database instance with proper initialization
        /// </summary>
        public Task<FirestoreDb> GetDatabaseAsync()
        {
            if (_database == null)
            {
                try
                {
                    string projectId = _configuration["GoogleCloud:ProjectId"] ?? throw new InvalidOperationException("GoogleCloud:ProjectId configuration is required");
                    string keyFilePath = _configuration["GoogleCloud:Credentials:Path"] ?? throw new InvalidOperationException("GoogleCloud:Credentials:Path configuration is required");

                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyFilePath);
                    _database = FirestoreDb.Create(projectId);
                    
                    _logger.LogDebug("Firestore database instance created for project {ProjectId}", projectId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create Firestore database instance");
                    throw;
                }
            }
            return Task.FromResult(_database);
        }

        /// <summary>
        /// Gets a collection reference with logging
        /// </summary>
        public async Task<CollectionReference> GetCollectionAsync(string collectionName)
        {
            try
            {
                var database = await GetDatabaseAsync();
                var collection = database.Collection(collectionName);
                _logger.LogDebug("Retrieved collection reference for {CollectionName}", collectionName);
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get collection {CollectionName}", collectionName);
                throw;
            }
        }

        /// <summary>
        /// Executes a transaction with return value
        /// </summary>
        public async Task<T> RunTransactionAsync<T>(Func<Transaction, Task<T>> operation, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = await GetDatabaseAsync();
                _logger.LogDebug("Starting Firestore transaction");
                
                var result = await database.RunTransactionAsync(operation);
                
                _logger.LogDebug("Firestore transaction completed successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firestore transaction failed");
                throw;
            }
        }

        /// <summary>
        /// Executes a transaction without return value
        /// </summary>
        public async Task RunTransactionAsync(Func<Transaction, Task> operation, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = await GetDatabaseAsync();
                _logger.LogDebug("Starting Firestore transaction");
                
                await database.RunTransactionAsync(operation);
                
                _logger.LogDebug("Firestore transaction completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firestore transaction failed");
                throw;
            }
        }

        /// <summary>
        /// Creates a batch operation
        /// </summary>
        public async Task<WriteBatch> CreateBatchAsync()
        {
            try
            {
                var database = await GetDatabaseAsync();
                var batch = database.StartBatch();
                _logger.LogDebug("Firestore batch operation created");
                return batch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Firestore batch");
                throw;
            }
        }

        /// <summary>
        /// Commits a batch operation
        /// </summary>
        public async Task CommitBatchAsync(WriteBatch batch, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Committing Firestore batch operation");
                await batch.CommitAsync();
                _logger.LogDebug("Firestore batch operation committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to commit Firestore batch");
                throw;
            }
        }
    }
}
