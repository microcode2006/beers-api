namespace Vintri.Beers.Core.Models;

public class QueryFilter
{
    public string? BeerName { get; set; }
    public int Page { get; set; } = Constants.DefaultPage;
    public int PerPage { get; set; } = Constants.DefaultPerPage;
}