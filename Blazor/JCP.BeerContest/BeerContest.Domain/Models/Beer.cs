using System;
using System.Collections.Generic;

namespace BeerContest.Domain.Models
{
    public class Beer
    {
        // Identification
        public string Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        
        // Beer characteristics (public data visible to judges)
        public string Description { get; set; }
        public double AlcoholByVolume { get; set; }
        public string Color { get; set; }
        public string Aroma { get; set; }
        public string Flavor { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public string BrewingProcess { get; set; }
        
        // Brewer information (private data not visible to judges)
        public string BrewerId { get; set; }
        public string BrewerName { get; set; }  // Not visible to judges
        public string BrewerEmail { get; set; } // Not visible to judges
        public string BrewerPhone { get; set; } // Not visible to judges
        
        // Contest related data
        public DateTime RegistrationDate { get; set; }
        public string ContestId { get; set; }
        public List<BeerRating> Ratings { get; set; } = new List<BeerRating>();
        
        // Helper method to get public data only (for judges)
        public Beer GetPublicData()
        {
            return new Beer
            {
                Id = this.Id,
                Name = this.Name,
                Style = this.Style,
                Description = this.Description,
                AlcoholByVolume = this.AlcoholByVolume,
                Color = this.Color,
                Aroma = this.Aroma,
                Flavor = this.Flavor,
                Ingredients = this.Ingredients,
                BrewingProcess = this.BrewingProcess,
                ContestId = this.ContestId,
                RegistrationDate = this.RegistrationDate
                // Note: Brewer personal information is excluded
            };
        }
    }

    public class BeerRating
    {
        public string Id { get; set; }
        public string BeerId { get; set; }
        public string JudgeId { get; set; }
        public int AppearanceScore { get; set; } // 1-5
        public int AromaScore { get; set; } // 1-5
        public int FlavorScore { get; set; } // 1-5
        public int MouthfeelScore { get; set; } // 1-5
        public int OverallImpressionScore { get; set; } // 1-5
        public string Comments { get; set; }
        public DateTime RatedAt { get; set; }
        
        public int TotalScore => AppearanceScore + AromaScore + FlavorScore + MouthfeelScore + OverallImpressionScore;
    }
}