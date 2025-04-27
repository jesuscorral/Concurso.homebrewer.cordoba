using BeerContest.Domain.Models;
using Google.Cloud.Firestore;
using System;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreBeerRating
    {
        [FirestoreProperty]
        public string Id { get; set; }
        
        [FirestoreProperty]
        public string BeerId { get; set; }
        
        [FirestoreProperty]
        public string JudgeId { get; set; }
        
        [FirestoreProperty]
        public int AppearanceScore { get; set; }
        
        [FirestoreProperty]
        public int AromaScore { get; set; }
        
        [FirestoreProperty]
        public int FlavorScore { get; set; }
        
        [FirestoreProperty]
        public int MouthfeelScore { get; set; }
        
        [FirestoreProperty]
        public int OverallImpressionScore { get; set; }
        
        [FirestoreProperty]
        public string Comments { get; set; }
        
        [FirestoreProperty]
        public DateTime RatedAt { get; set; }

        // Convert from domain model to Firestore model
        public static FirestoreBeerRating FromBeerRating(BeerRating rating)
        {
            return new FirestoreBeerRating
            {
                Id = rating.Id,
                BeerId = rating.BeerId,
                JudgeId = rating.JudgeId,
                AppearanceScore = rating.AppearanceScore,
                AromaScore = rating.AromaScore,
                FlavorScore = rating.FlavorScore,
                MouthfeelScore = rating.MouthfeelScore,
                OverallImpressionScore = rating.OverallImpressionScore,
                Comments = rating.Comments,
                RatedAt = rating.RatedAt
            };
        }

        // Convert from Firestore model to domain model
        public BeerRating ToBeerRating()
        {
            return new BeerRating
            {
                Id = Id,
                BeerId = BeerId,
                JudgeId = JudgeId,
                AppearanceScore = AppearanceScore,
                AromaScore = AromaScore,
                FlavorScore = FlavorScore,
                MouthfeelScore = MouthfeelScore,
                OverallImpressionScore = OverallImpressionScore,
                Comments = Comments,
                RatedAt = RatedAt
            };
        }
    }
}