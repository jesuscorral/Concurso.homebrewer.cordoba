namespace BeerContest.Domain.Models
{
    public class Beer
    {
        public string Id { get; set; }
        public BeerCategory Category { get; set; }
        public string BeerStyle { get; set; }
        public double AlcoholContent { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime BottleDate { get; set; }
        public string Malts { get; set; }
        public string Hops { get; set; }
        public string Yeast { get; set; }
        public string Additives { get; set; }

        // Entry instructions
        public string EntryInstructions { get; set; }

        //TODO: Helper method to get public data only (for judges)
        //public Beer GetPublicData()
        //{
        //    return new Beer
        //    {
        //        Id = this.Id,
        //        Name = this.Name,
        //        Style = this.Style,
        //        Description = this.Description,
        //        AlcoholByVolume = this.AlcoholByVolume,
        //        Color = this.Color,
        //        Aroma = this.Aroma,
        //        Flavor = this.Flavor,
        //        Ingredients = this.Ingredients,
        //        BrewingProcess = this.BrewingProcess,
        //        ContestId = this.ContestId,
        //        RegistrationDate = this.RegistrationDate
        //        // Note: Brewer personal information is excluded
        //    };
        //}
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