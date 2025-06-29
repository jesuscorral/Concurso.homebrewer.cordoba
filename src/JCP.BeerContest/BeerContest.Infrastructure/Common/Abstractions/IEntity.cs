namespace BeerContest.Infrastructure.Common.Abstractions
{
    /// <summary>
    /// Base interface for entities with common properties
    /// </summary>
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    /// <summary>
    /// Interface for entities with audit information
    /// </summary>
    public interface IAuditableEntity<TKey> : IEntity<TKey>
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
    }

    /// <summary>
    /// Interface for soft-deletable entities
    /// </summary>
    public interface ISoftDeletableEntity<TKey> : IEntity<TKey>
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
    }
}
