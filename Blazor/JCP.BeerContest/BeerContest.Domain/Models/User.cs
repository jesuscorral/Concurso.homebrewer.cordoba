using System;
using System.Collections.Generic;

namespace BeerContest.Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }
        public List<Beer> RegisteredBeers { get; set; } = new List<Beer>();
        public List<Beer> AssignedBeersForJudging { get; set; } = new List<Beer>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public enum UserRole
    {
        Participant,
        Judge,
        Administrator
    }
}