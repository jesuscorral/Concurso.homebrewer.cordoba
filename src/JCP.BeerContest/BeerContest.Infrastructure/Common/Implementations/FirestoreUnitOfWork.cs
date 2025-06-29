using BeerContest.Infrastructure.Common.Abstractions;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Common.Implementations
{
    /// <summary>
    /// Unit of Work implementation for Firestore transactions
    /// </summary>
    public class FirestoreUnitOfWork : IUnitOfWork
    {
        private readonly IFirestoreContext _context;
        private readonly ILogger<FirestoreUnitOfWork> _logger;
        private Transaction? _currentTransaction;
        private bool _disposed = false;

        public FirestoreUnitOfWork(IFirestoreContext context, ILogger<FirestoreUnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                _logger.LogWarning("Transaction already in progress");
                return;
            }

            try
            {
                var database = await _context.GetDatabaseAsync();
                // Note: Firestore doesn't have explicit begin transaction like SQL
                // Transactions are created within RunTransactionAsync
                _logger.LogDebug("Transaction context prepared");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to begin transaction");
                throw;
            }
        }

        public async Task CommitAsync()
        {
            // In Firestore, commits happen automatically at the end of RunTransactionAsync
            _logger.LogDebug("Transaction committed");
            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            // In Firestore, rollbacks happen automatically if exceptions occur
            _logger.LogDebug("Transaction rolled back");
            await Task.CompletedTask;
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                _logger.LogDebug("Executing operation in transaction");
                
                return await _context.RunTransactionAsync(async transaction =>
                {
                    _currentTransaction = transaction;
                    try
                    {
                        return await operation();
                    }
                    finally
                    {
                        _currentTransaction = null;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction operation failed");
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            try
            {
                _logger.LogDebug("Executing operation in transaction");
                
                await _context.RunTransactionAsync(async transaction =>
                {
                    _currentTransaction = transaction;
                    try
                    {
                        await operation();
                    }
                    finally
                    {
                        _currentTransaction = null;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction operation failed");
                throw;
            }
        }

        public async Task<IBatchOperation> CreateBatchAsync()
        {
            var batch = await _context.CreateBatchAsync();
            return new FirestoreBatchOperation(batch, _context, _logger);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _currentTransaction = null;
                _disposed = true;
                _logger.LogDebug("Unit of Work disposed");
            }
        }
    }

    /// <summary>
    /// Firestore batch operation implementation
    /// </summary>
    internal class FirestoreBatchOperation : IBatchOperation
    {
        private readonly WriteBatch _batch;
        private readonly IFirestoreContext _context;
        private readonly ILogger _logger;
        private bool _disposed = false;

        public FirestoreBatchOperation(WriteBatch batch, IFirestoreContext context, ILogger logger)
        {
            _batch = batch;
            _context = context;
            _logger = logger;
        }

        public void Add<T>(string collectionName, T entity) where T : class
        {
            try
            {
                var collection = _context.GetCollectionAsync(collectionName).Result;
                var documentRef = collection.Document(); // Auto-generate ID
                _batch.Create(documentRef, entity);
                _logger.LogDebug("Added entity to batch for collection {CollectionName}", collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add entity to batch for collection {CollectionName}", collectionName);
                throw;
            }
        }

        public void Update<T>(string collectionName, string documentId, T entity) where T : class
        {
            try
            {
                var collection = _context.GetCollectionAsync(collectionName).Result;
                var documentRef = collection.Document(documentId);
                _batch.Set(documentRef, entity);
                _logger.LogDebug("Added update to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add update to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        public void UpdateFields(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            try
            {
                var collection = _context.GetCollectionAsync(collectionName).Result;
                var documentRef = collection.Document(documentId);
                _batch.Update(documentRef, updates);
                _logger.LogDebug("Added field update to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add field update to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        public void Delete(string collectionName, string documentId)
        {
            try
            {
                var collection = _context.GetCollectionAsync(collectionName).Result;
                var documentRef = collection.Document(documentId);
                _batch.Delete(documentRef);
                _logger.LogDebug("Added delete to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add delete to batch for document {DocumentId} in collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.CommitBatchAsync(_batch);
                _logger.LogDebug("Batch operation committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to commit batch operation");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // WriteBatch doesn't implement IDisposable, so nothing to dispose
                _disposed = true;
                _logger.LogDebug("Batch operation disposed");
            }
        }
    }
}
