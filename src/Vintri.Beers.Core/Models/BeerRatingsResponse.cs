using System.Collections.Generic;

namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Response model for the data returned from GetBeersAsync action
    /// </summary>
    public record BeerRatingsResponse(int Id, string Name, string Description, List<UserRating> UserRatings)
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
        public string Description { get; } = Description;
        public List<UserRating> UserRatings { get; } = UserRatings;
    }
}