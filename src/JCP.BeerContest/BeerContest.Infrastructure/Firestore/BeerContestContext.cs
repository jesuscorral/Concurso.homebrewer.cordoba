using BeerContest.Infrastructure.Services;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore
{
    public class BeerContestContext
    {
        private readonly ISecureFirebaseService _firebaseService;
        private FirestoreDb? _firestoreDb;

        public BeerContestContext(ISecureFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        private async Task<FirestoreDb> GetFirestoreDbAsync()
        {
            if (_firestoreDb == null)
            {
                _firestoreDb = await _firebaseService.GetFirestoreDbAsync();
            }
            return _firestoreDb;
        }

        public async Task<CollectionReference> GetUsersCollectionAsync()
        {
            var db = await GetFirestoreDbAsync();
            return db.Collection("users");
        }

        public async Task<CollectionReference> GetBeersCollectionAsync()
        {
            var db = await GetFirestoreDbAsync();
            return db.Collection("beers");
        }

        public async Task<CollectionReference> GetContestsCollectionAsync()
        {
            var db = await GetFirestoreDbAsync();
            return db.Collection("contests");
        }
        public async Task<T?> GetDocumentAsync<T>(string collectionName, string documentId) where T : class
        {
            var db = await GetFirestoreDbAsync();
            DocumentReference docRef = db.Collection(collectionName).Document(documentId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                try
                {
                    return snapshot.ConvertTo<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting document: {ex.Message}");
                    throw;
                }
            }

            return null;
        }        
        
        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string collectionName) where T : class
        {
            var db = await GetFirestoreDbAsync();
            Query query = db.Collection(collectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            List<T> items = new List<T>();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    try
                    {
                        items.Add(documentSnapshot.ConvertTo<T>());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error converting document: {ex.Message}");
                        // Skip this document and continue with others
                    }
                }
            }

            return items;
        }

        public async Task<string> AddDocumentAsync<T>(string collectionName, T item)
        {
            try
            {
                var db = await GetFirestoreDbAsync();
                CollectionReference colRef = db.Collection(collectionName);
                DocumentReference docRef = await colRef.AddAsync(item);
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding document: {ex.Message}");
                throw;
            }
        }

        public async Task SetDocumentAsync<T>(string collectionName, string documentId, T item)
        {
            try
            {
                var db = await GetFirestoreDbAsync();
                DocumentReference docRef = db.Collection(collectionName).Document(documentId);
                await docRef.SetAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting document: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDocumentAsync(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            var db = await GetFirestoreDbAsync();
            DocumentReference docRef = db.Collection(collectionName).Document(documentId);
            await docRef.UpdateAsync(updates);
        }

        public async Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            var db = await GetFirestoreDbAsync();
            DocumentReference docRef = db.Collection(collectionName).Document(documentId);
            await docRef.DeleteAsync();
        }        
        
        public async Task<Query> CreateQueryAsync(string collectionName)
        {
            var db = await GetFirestoreDbAsync();
            return db.Collection(collectionName);
        }
    }
}