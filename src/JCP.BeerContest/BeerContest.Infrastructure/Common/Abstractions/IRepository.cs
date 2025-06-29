using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Common.Abstractions
{
    /// <summary>
    /// Base interface for all repository operations with common CRUD functionality
    /// </summary>
    /// <typeparam name="TEntity">Domain entity type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities from the repository
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities based on a predicate
        /// </summary>
        Task<IEnumerable<TEntity>> FindAsync(Func<Query, Query> queryBuilder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity based on a predicate
        /// </summary>
        Task<TEntity?> FirstOrDefaultAsync(Func<Query, Query> queryBuilder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<TKey> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Partially updates an entity with specific fields
        /// </summary>
        Task UpdateFieldsAsync(TKey id, Dictionary<string, object> updates, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by its identifier
        /// </summary>
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity exists
        /// </summary>
        Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the count of entities matching a condition
        /// </summary>
        Task<int> CountAsync(Func<Query, Query>? queryBuilder = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities with pagination
        /// </summary>
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            Func<Query, Query>? queryBuilder = null, 
            CancellationToken cancellationToken = default);
    }
}
