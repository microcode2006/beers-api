namespace Vintri.Beers.Core;

public static class Constants
{
    public const string ApiVersion = "apiVersion";

    public const int DefaultPage = 1;
    public const int DefaultPerPage = 5;
    public const int MaxPerPage = 25;

    public const int PunkClientLifetimeInMinutes = 5;
    public const int PunkClientMaxRetryCount = 3;
    public const int PunkClientBackoffBase = 2;
}