namespace Vintri.Beers.Core.Models
{
    /// <summary>
    /// Response model for beer that's returned from Punk API
    /// </summary>
    public class Beer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}