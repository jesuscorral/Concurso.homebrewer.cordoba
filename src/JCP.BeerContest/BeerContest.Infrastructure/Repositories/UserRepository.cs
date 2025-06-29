using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    /// <summary>
    /// Enhanced User repository using the new base repository pattern
    /// </summary>
    public class UserRepository : FirestoreRepositoryBase<User, FirestoreUser, string>, IUserRepository
    {
        protected override string CollectionName => "users";

        public UserRepository(
            IFirestoreContext context, 
            ILogger<UserRepository> logger) 
            : base(context, logger)
        {
        }

        protected override User ToDomainEntity(FirestoreUser firestoreModel)
        {
            return firestoreModel.ToUser();
        }

        protected override FirestoreUser ToFirestoreModel(User entity)
        {
            return FirestoreUser.FromUser(entity);
        }

        protected override string KeyToString(string key) => key;

        protected override string StringToKey(string documentId) => documentId;

        // IUserRepository specific methods
        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                _logger.LogDebug("Getting user by email: {Email}", email);
                
                var users = await FindAsync(query => query.WhereEqualTo("Email", email));
                var user = users.FirstOrDefault();
                
                return user ?? throw new KeyNotFoundException($"User with email {email} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                throw;
            }
        }

        public async Task<string> CreateAsync(User user)
        {
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(user);
                _logger.LogInformation("Created user {UserId} with email {Email}", id, user.Email);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email {Email}", user.Email);
                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                await UpdateAsync(user.Id, user);
                _logger.LogInformation("Updated user {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", user.Id);
                throw;
            }
        }

        // Explicit implementations for IUserRepository (delegating to base class)
        async Task<User?> IUserRepository.GetByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        async Task<IEnumerable<User>> IUserRepository.GetAllAsync()
        {
            return await GetAllAsync();
        }
    }
}
