using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Services
{
    /// <summary>
    /// Secure Firebase configuration service that loads credentials from Azure Key Vault
    /// instead of local files, following Azure security best practices.
    /// </summary>
    public interface ISecureFirebaseService
    {
        Task<FirestoreDb> GetFirestoreDbAsync();
        Task InitializeAsync();
    }

    public class SecureFirebaseService : ISecureFirebaseService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecureFirebaseService> _logger;
        private FirestoreDb _firestoreDb;
        private bool _initialized = false;

        public SecureFirebaseService(IConfiguration configuration, ILogger<SecureFirebaseService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            try
            {
                await InitializeFromKeyVaultAsync();

                _initialized = true;
                _logger.LogInformation("Firebase service initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Firebase service");
                throw;
            }
        }

        private async Task InitializeFromKeyVaultAsync()
        {
            try
            {
                var projectId = _configuration[Constants.GoogleCloud.PROJECT_ID] ?? throw new InvalidOperationException("GoogleCloud:ProjectId configuration is required");
                var serviceAccountJson = _configuration[Constants.Firebase.CREDENTIALS] ?? throw new InvalidOperationException("Firebase service account credentials are required in Key Vault");
                var tempPath = Path.GetTempFileName();
                await File.WriteAllTextAsync(tempPath, serviceAccountJson);

                // Set environment variable for Google Cloud SDK
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

                // TODO - Intentar inicializar de esta forma para no crear archivos temporales

                //var firebaseCredentialsJson = _configuration[Constants.Firebase.CREDENTIALS] ?? throw new InvalidOperationException("Firebase service account credentials are required in Key Vault");
                //var googleCredential = GoogleCredential.FromJson(firebaseCredentialsJson);
                //FirebaseApp.Create(new AppOptions()
                //{
                //    Credential = googleCredential
                //});

                // Initialize Firestore
                _firestoreDb = FirestoreDb.Create(projectId);

                _logger.LogInformation("Firebase initialized with credentials from Key Vault");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Firebase credentials from Key Vault");
                throw;
            }
        }

        public async Task<FirestoreDb> GetFirestoreDbAsync()
        {
            if (!_initialized)
            {
                await InitializeAsync();
            }

            return _firestoreDb ?? throw new InvalidOperationException("Firebase service not properly initialized");
        }
    }
}
