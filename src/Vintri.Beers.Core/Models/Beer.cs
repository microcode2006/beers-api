namespace Vintri.Beers.Core.Models;

/// <summary>
/// Response model for beer that's returned from Punk API
/// Nullable reference model design: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
/// </summary>
public record Beer(int Id, string Name, string Description)
{
    public string Name { get; } = Name;
    public string Description { get; } = Description;
    public int Id { get; } = Id;
}