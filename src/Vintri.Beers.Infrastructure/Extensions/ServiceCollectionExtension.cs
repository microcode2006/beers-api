using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Infrastructure.Loggers;
using Vintri.Beers.Infrastructure.Repositories;

namespace Vintri.Beers.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPunkClient(this IServiceCollection serviceCollection) =>
        serviceCollection.AddHttpClient<IPunkClient, PunkClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(Constants.PunkClientLifetimeInMinutes))
            .AddPolicyHandler(GetRetryPolicy());

    public static void AddUserRatingRepository(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<IBeerRatingRepository, BeerRatingRepository>();

    public static void AddSerilog(this IServiceCollection serviceCollection)
    {
        var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "logs.txt");
        serviceCollection.AddSingleton<Serilog.ILogger>(new LoggerConfiguration().WriteTo
            .File(logFilePath, rollingInterval: RollingInterval.Day).CreateLogger());

        serviceCollection.AddScoped<IBeersLogger, SeriBeerslogger>();
    }


    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Constants.PunkClientMaxRetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(Constants.PunkClientBackoffBase, retryAttempt)));

}