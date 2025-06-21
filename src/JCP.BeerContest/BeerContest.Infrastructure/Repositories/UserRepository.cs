using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Firestore;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BeerContestContext _firestoreContext;
        private const string CollectionName = "users";

        public UserRepository(BeerContestContext firestoreContext)
        {
            _firestoreContext = firestoreContext;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            var user = await _firestoreContext.GetDocumentAsync<FirestoreUser>(CollectionName, id);
            return user?.ToUser();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            Query query = _firestoreContext.CreateQuery(CollectionName)
                .WhereEqualTo("Email", email);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            var firestoreUsers = querySnapshot.Documents
                .Select(d => d.ConvertTo<FirestoreUser>())
                .FirstOrDefault();
            return firestoreUsers?.ToUser();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var firestoreUsers = await _firestoreContext.GetCollectionAsync<FirestoreUser>(CollectionName);
            return firestoreUsers.Select(fb => fb.ToUser());
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
            var firestoreUser = FirestoreUser.FromUser(user);
            var id = await _firestoreContext.AddDocumentAsync(CollectionName, firestoreUser);
            return id;
        }

        public Task UpdateAsync(User user)
        {
            var firestoreUser = FirestoreUser.FromUser(user);
            return _firestoreContext.SetDocumentAsync(CollectionName, user.Id, firestoreUser);
        }

        public Task DeleteAsync(string id)
        {
            return _firestoreContext.DeleteDocumentAsync(CollectionName, id);
        }
    }
}