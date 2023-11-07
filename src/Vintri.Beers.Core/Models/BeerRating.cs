namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Request model for user to add rating for a beer
    /// </summary>
    public class BeerRating {
        public int BeerId { get; set; }
        public UserRating? UserRating { get; set; }
    }
}