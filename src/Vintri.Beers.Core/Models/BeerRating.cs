namespace Vintri.Beers.Core.Models;

/// <summary>
/// Request model for user to add rating for a beer
/// </summary>
public record BeerRating(int BeerId, UserRating UserRating)
{
    public int BeerId { get; } = BeerId;
    public UserRating UserRating { get; } = UserRating;
}