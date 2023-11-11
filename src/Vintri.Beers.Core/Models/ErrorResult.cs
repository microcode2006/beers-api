namespace Vintri.Beers.Core.Models;

public record ErrorResult
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}