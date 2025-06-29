using BeerContest.Domain.Models;
using Google.Cloud.Firestore;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreUser
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string GoogleId { get; set; }

        [FirestoreProperty]
        public required string Email { get; set; }

        [FirestoreProperty]
        public required string DisplayName { get; set; }

        [FirestoreProperty]
        public required List<int> RoleIds { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? LastLoginAt { get; set; }

        // Convert from domain model to Firestore model
        public static FirestoreUser FromUser(User user)
        {
            return new FirestoreUser
            {
                Id = user.Id,
                GoogleId = user.GoogleId,
                Email = user.Email,
                DisplayName = user.DisplayName,
                RoleIds = user.Roles.Select(role => (int)role).ToList(),
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
                GoogleId = GoogleId,
                Email = Email,
                DisplayName = DisplayName,
                Roles = RoleIds.Select(roleId => (UserRole)roleId).ToList(), 
                CreatedAt = CreatedAt.ToLocalTime(),
                LastLoginAt = LastLoginAt?.ToLocalTime()
            };
        }
    }
}
