using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Its entity model for json file repository
    /// </summary>
    public class BeerRatings
    {
        public int Id { get; set; }
        public List<UserRating>? UserRatings { get; set; }
    }
}