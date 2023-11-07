using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    public class BeerRatings
    {
        public int Id { get; set; }
        public List<UserRating>? UserRatings { get; set; }
    }
}