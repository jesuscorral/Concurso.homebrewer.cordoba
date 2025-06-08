using BeerContest.Domain.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace BeerContest.Infrastructure.Firestore.FirestoreModels
{
    [FirestoreData]
    public class FirestoreJudgingTable
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string ContestId { get; set; }

        [FirestoreProperty]
        public List<string> JudgeIds { get; set; } = new List<string>();

        [FirestoreProperty]
        public List<string> BeerIds { get; set; } = new List<string>();

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? UpdatedAt { get; set; }

        public static FirestoreJudgingTable FromJudgingTable(JudgingTable table)
        {
            return new FirestoreJudgingTable
            {
                Id = table.Id,
                Name = table.Name,
                ContestId = table.ContestId,
                JudgeIds = table.JudgeIds,
                BeerIds = table.BeerIds,
                CreatedAt = table.CreatedAt.ToUniversalTime(),
                UpdatedAt = table.UpdatedAt?.ToUniversalTime()
            };
        }

        public JudgingTable ToJudgingTable()
        {
            return new JudgingTable
            {
                Id = Id,
                Name = Name,
                ContestId = ContestId,
                JudgeIds = JudgeIds,
                BeerIds = BeerIds,
                CreatedAt = CreatedAt.ToLocalTime(),
                UpdatedAt = UpdatedAt?.ToLocalTime()
            };
        }
    }
}