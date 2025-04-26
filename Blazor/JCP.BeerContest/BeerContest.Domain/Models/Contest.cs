using System;
using System.Collections.Generic;

namespace BeerContest.Domain.Models
{
    public class Contest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string OrganizerId { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public List<ContestRule> Rules { get; set; } = new List<ContestRule>();
        public List<BeerCategory> Categories { get; set; } = new List<BeerCategory>();
        public ContestStatus Status { get; set; }
        public int MaxBeersPerParticipant { get; set; } = 3; // Default limit of 3 beers per participant
    }

    public class ContestRule
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class BeerCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public enum ContestStatus
    {
        Draft,
        RegistrationOpen,
        RegistrationClosed,
        Judging,
        Completed
    }
}