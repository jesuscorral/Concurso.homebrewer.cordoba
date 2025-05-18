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

        public string ParticipantEmail { get; set; }
        public string EntryInstructions { get; set; }
        public DateTime CreatedAt { get; set; }


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

 
}