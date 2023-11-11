using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Its entity model for json file repository
    /// </summary>
    public record BeerRatings(int Id, List<UserRating> UserRatings)
    {
        public int Id { get; } = Id;
        public List<UserRating> UserRatings { get; } = UserRatings;
    }
}