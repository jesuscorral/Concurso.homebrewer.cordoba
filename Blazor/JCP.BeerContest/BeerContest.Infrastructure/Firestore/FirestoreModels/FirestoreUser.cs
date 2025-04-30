using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreUser
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string DisplayName { get; set; }

        [FirestoreProperty]
        public int RoleId { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? LastLoginAt { get; set; }

        // Convert from domain model to Firestore model
        public static FirestoreUser FromUser(User user)
        {
            return new FirestoreUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = user.Email,
                DisplayName = user.DisplayName,
                RoleId = (int)user.Role,
                CreatedAt = user.CreatedAt.ToUniversalTime(),
                LastLoginAt = user.LastLoginAt?.ToUniversalTime()
            };
        }

        // Convert from Firestore model to domain model
        public User ToUser()
        {
            return new User
            {
                Id = Id,
                Email = Email,
                DisplayName = DisplayName,
                Role = (UserRole)RoleId,
                CreatedAt = CreatedAt.ToLocalTime(),
                LastLoginAt = LastLoginAt?.ToLocalTime()
            };
        }
    }
}
