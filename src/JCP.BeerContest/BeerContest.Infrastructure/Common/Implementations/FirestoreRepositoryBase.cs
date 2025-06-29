using BeerContest.Infrastructure.Common.Abstractions;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Common.Implementations
{
    /// <summary>
    /// Base repository implementation with common Firestore operations
    /// </summary>
    /// <typeparam name="TEntity">Domain entity type</typeparam>
    /// <typeparam name="TFirestoreModel">Firestore model type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public abstract class FirestoreRepositoryBase<TEntity, TFirestoreModel, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
        where TFirestoreModel : class
    {
        protected readonly IFirestoreContext _context;
        protected readonly ILogger _logger;
        protected abstract string CollectionName { get; }

        protected FirestoreRepositoryBase(IFirestoreContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Converts Firestore model to domain entity
        /// </summary>
        protected abstract TEntity ToDomainEntity(TFirestoreModel firestoreModel);

        /// <summary>
        /// Converts domain entity to Firestore model
        /// </summary>
        protected abstract TFirestoreModel ToFirestoreModel(TEntity entity);

        /// <summary>
        /// Converts key to string for Firestore document ID
        /// </summary>
        protected abstract string KeyToString(TKey key);

        /// <summary>
        /// Converts string document ID to key
        /// </summary>
        protected abstract TKey StringToKey(string documentId);

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var documentId = KeyToString(id);
                var document = await collection.Document(documentId).GetSnapshotAsync(cancellationToken);

                if (!document.Exists)
                {
                    _logger.LogDebug("Document {DocumentId} not found in collection {CollectionName}", documentId, CollectionName);
                    return null;
                }

                var firestoreModel = document.ConvertTo<TFirestoreModel>();
                var entity = ToDomainEntity(firestoreModel);
                
                _logger.LogDebug("Retrieved entity {EntityId} from collection {CollectionName}", id, CollectionName);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get entity {EntityId} from collection {CollectionName}", id, CollectionName);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var snapshot = await collection.GetSnapshotAsync(cancellationToken);

                var entities = snapshot.Documents
                    .Where(doc => doc.Exists)
                    .Select(doc => {
                        try
                        {
                            var firestoreModel = doc.ConvertTo<TFirestoreModel>();
                            return ToDomainEntity(firestoreModel);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to convert document {DocumentId} in collection {CollectionName}", doc.Id, CollectionName);
                            return null;
                        }
                    })
                    .Where(entity => entity != null)
                    .Cast<TEntity>()
                    .ToList();

                _logger.LogDebug("Retrieved {Count} entities from collection {CollectionName}", entities.Count(), CollectionName);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all entities from collection {CollectionName}", CollectionName);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Func<Query, Query> queryBuilder, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var query = queryBuilder(collection);
                var snapshot = await query.GetSnapshotAsync(cancellationToken);

                var entities = snapshot.Documents
                    .Where(doc => doc.Exists)
                    .Select(doc => {
                        try
                        {
                            var firestoreModel = doc.ConvertTo<TFirestoreModel>();
                            return ToDomainEntity(firestoreModel);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to convert document {DocumentId} in collection {CollectionName}", doc.Id, CollectionName);
                            return null;
                        }
                    })
                    .Where(entity => entity != null)
                    .Cast<TEntity>()
                    .ToList();

                _logger.LogDebug("Found {Count} entities in collection {CollectionName} with custom query", entities.Count(), CollectionName);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute find query in collection {CollectionName}", CollectionName);
                throw;
            }
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Func<Query, Query> queryBuilder, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var query = queryBuilder(collection).Limit(1);
                var snapshot = await query.GetSnapshotAsync(cancellationToken);

                var document = snapshot.Documents.FirstOrDefault();
                if (document?.Exists == true)
                {
                    var firestoreModel = document.ConvertTo<TFirestoreModel>();
                    var entity = ToDomainEntity(firestoreModel);
                    _logger.LogDebug("Found first entity in collection {CollectionName} with custom query", CollectionName);
                    return entity;
                }

                _logger.LogDebug("No entity found in collection {CollectionName} with custom query", CollectionName);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute first or default query in collection {CollectionName}", CollectionName);
                throw;
            }
        }

        public virtual async Task<TKey> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var firestoreModel = ToFirestoreModel(entity);
                var documentRef = await collection.AddAsync(firestoreModel, cancellationToken);
                
                var key = StringToKey(documentRef.Id);
                _logger.LogDebug("Added entity with ID {EntityId} to collection {CollectionName}", key, CollectionName);
                return key;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add entity to collection {CollectionName}", CollectionName);
                throw;
            }
        }

        public virtual async Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var documentId = KeyToString(id);
                var firestoreModel = ToFirestoreModel(entity);
                
                await collection.Document(documentId).SetAsync(firestoreModel, cancellationToken: cancellationToken);
                _logger.LogDebug("Updated entity {EntityId} in collection {CollectionName}", id, CollectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update entity {EntityId} in collection {CollectionName}", id, CollectionName);
                throw;
            }
        }

        public virtual async Task UpdateFieldsAsync(TKey id, Dictionary<string, object> updates, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var documentId = KeyToString(id);
                
                await collection.Document(documentId).UpdateAsync(updates, cancellationToken: cancellationToken);
                _logger.LogDebug("Updated fields for entity {EntityId} in collection {CollectionName}", id, CollectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update fields for entity {EntityId} in collection {CollectionName}", id, CollectionName);
                throw;
            }
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var documentId = KeyToString(id);
                
                await collection.Document(documentId).DeleteAsync(cancellationToken: cancellationToken);
                _logger.LogDebug("Deleted entity {EntityId} from collection {CollectionName}", id, CollectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete entity {EntityId} from collection {CollectionName}", id, CollectionName);
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var documentId = KeyToString(id);
                var document = await collection.Document(documentId).GetSnapshotAsync(cancellationToken);
                
                var exists = document.Exists;
                _logger.LogDebug("Entity {EntityId} exists in collection {CollectionName}: {Exists}", id, CollectionName, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check existence of entity {EntityId} in collection {CollectionName}", id, CollectionName);
                throw;
            }
        }

        public virtual async Task<int> CountAsync(Func<Query, Query>? queryBuilder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var query = queryBuilder?.Invoke(collection) ?? collection;
                var snapshot = await query.GetSnapshotAsync(cancellationToken);
                
                var count = snapshot.Count;
                _logger.LogDebug("Count for collection {CollectionName}: {Count}", CollectionName, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get count for collection {CollectionName}", CollectionName);
                throw;
            }
        }

        public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            Func<Query, Query>? queryBuilder = null, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = await _context.GetCollectionAsync(CollectionName);
                var baseQuery = queryBuilder?.Invoke(collection) ?? collection;
                
                // Get total count
                var countSnapshot = await baseQuery.GetSnapshotAsync(cancellationToken);
                var totalCount = countSnapshot.Count;
                
                // Get paged results
                var offset = (pageNumber - 1) * pageSize;
                var pagedQuery = baseQuery.Offset(offset).Limit(pageSize);
                var pagedSnapshot = await pagedQuery.GetSnapshotAsync(cancellationToken);
                
                var entities = pagedSnapshot.Documents
                    .Where(doc => doc.Exists)
                    .Select(doc => {
                        try
                        {
                            var firestoreModel = doc.ConvertTo<TFirestoreModel>();
                            return ToDomainEntity(firestoreModel);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to convert document {DocumentId} in collection {CollectionName}", doc.Id, CollectionName);
                            return null;
                        }
                    })
                    .Where(entity => entity != null)
                    .Cast<TEntity>()
                    .ToList();

                _logger.LogDebug("Retrieved page {PageNumber} with {Count} entities from collection {CollectionName} (Total: {TotalCount})", 
                    pageNumber, entities.Count(), CollectionName, totalCount);
                
                return (entities, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get paged results for collection {CollectionName}", CollectionName);
                throw;
            }
        }
    }
}
