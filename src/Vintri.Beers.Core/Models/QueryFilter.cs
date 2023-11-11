namespace Vintri.Beers.Core.Models;

public record QueryFilter
{
    public string BeerName { get; set; } = string.Empty;
    public int Page { get; set; } = Constants.DefaultPage;
    public int PerPage { get; set; } = Constants.DefaultPerPage;
}