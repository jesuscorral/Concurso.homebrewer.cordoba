using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace BeerContest.Infrastructure.Firestore
{
    public class FirestoreContext
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreContext(IConfiguration configuration)
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
                return snapshot.ConvertTo<T>();
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
                    items.Add(documentSnapshot.ConvertTo<T>());
                }
            }

            return items;
        }

        public async Task<string> AddDocumentAsync<T>(string collectionName, T item)
        {
            CollectionReference colRef = _firestoreDb.Collection(collectionName);
            DocumentReference docRef = await colRef.AddAsync(item);
            return docRef.Id;
        }

        public Task SetDocumentAsync<T>(string collectionName, string documentId, T item)
        {
            DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            return docRef.SetAsync(item);
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