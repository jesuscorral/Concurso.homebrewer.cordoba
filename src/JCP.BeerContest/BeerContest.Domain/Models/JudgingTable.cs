using System;
using System.Collections.Generic;

namespace BeerContest.Domain.Models
{
    public class JudgingTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContestId { get; set; }
        public List<string> JudgeIds { get; set; } = new List<string>();
        public List<string> BeerIds { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}