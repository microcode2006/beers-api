namespace Vintri.Beers.Core.Models
{
    public record UserRating(string Username, int Rating)
    {
        public string Username { get; } = Username;
        public int Rating { get; } = Rating;
        public string? Comments { get; set; }
    }
}