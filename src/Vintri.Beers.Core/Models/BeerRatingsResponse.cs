using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Response model for the data returned from GetBeersAsync action
    /// </summary>
    public class BeerRatingsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<UserRating>? UserRatings { get; set; }
    }
}