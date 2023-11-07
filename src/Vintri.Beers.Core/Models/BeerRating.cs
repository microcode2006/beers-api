namespace Vintri.Beers.Core.Models
{
    public class BeerRating {
        public int BeerId { get; set; }
        public UserRating? UserRating { get; set; }
    }
}