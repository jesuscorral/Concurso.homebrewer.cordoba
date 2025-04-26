using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreContext _firestoreContext;
        private const string CollectionName = "users";

        public UserRepository(FirestoreContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _firestoreContext.GetDocumentAsync<User>(CollectionName, id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            Query query = _firestoreContext.CreateQuery(CollectionName)
                .WhereEqualTo("Email", email);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<User>())
                .FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _firestoreContext.GetCollectionAsync<User>(CollectionName);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            Query query = _firestoreContext.CreateQuery(CollectionName)
                .WhereEqualTo("Role", role);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents
                .Select(d => d.ConvertTo<User>())
                .ToList();
        }

        public async Task<string> CreateAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            return await _firestoreContext.AddDocumentAsync(CollectionName, user);
        }

        public Task UpdateAsync(User user)
        {
            return _firestoreContext.SetDocumentAsync(CollectionName, user.Id, user);
        }

        public Task DeleteAsync(string id)
        {
            return _firestoreContext.DeleteDocumentAsync(CollectionName, id);
        }
    }
}