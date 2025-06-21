using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace BeerContest.Infrastructure.Firestore
{
    public class BeerContestContext
    {
        private readonly FirestoreDb _firestoreDb;

        public BeerContestContext(IConfiguration configuration)
        {
            string projectId = configuration["GoogleCloud:ProjectId"];
            string keyFilePath = configuration["GoogleCloud:Credentials:Path"];

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyFilePath);


            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public CollectionReference UsersCollection => _firestoreDb.Collection("users");
        public CollectionReference BeersCollection => _firestoreDb.Collection("beers");
        public CollectionReference ContestsCollection => _firestoreDb.Collection("contests");

        public async Task<T> GetDocumentAsync<T>(string collectionName, string documentId) where T : class
        {
            DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
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
            Query query = _firestoreDb.Collection(collectionName);
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
                CollectionReference colRef = _firestoreDb.Collection(collectionName);
                DocumentReference docRef = await colRef.AddAsync(item);
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding document: {ex.Message}");
                throw;
            }
        }

        public Task SetDocumentAsync<T>(string collectionName, string documentId, T item)
        {
            try
            {
                DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
                return docRef.SetAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting document: {ex.Message}");
                throw;
            }
        }

        public Task UpdateDocumentAsync(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            return docRef.UpdateAsync(updates);
        }

        public Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            return docRef.DeleteAsync();
        }

        public Query CreateQuery(string collectionName)
        {
            return _firestoreDb.Collection(collectionName);
        }
    }
}