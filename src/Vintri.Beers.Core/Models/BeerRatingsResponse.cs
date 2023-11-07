using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    public class BeerRatingsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<UserRating>? UserRatings { get; set; }
    }
}