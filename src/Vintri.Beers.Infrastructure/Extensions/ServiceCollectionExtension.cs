using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Infrastructure.Repositories;

namespace Vintri.Beers.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPunkClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IPunkClient, PunkClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(Constants.PunkClientLifetimeInMinutes))
            .AddPolicyHandler(GetRetryPolicy());
    }

    public static void AddUserRatingRepository(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRatingRepository, UserRatingRepository>();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Constants.PunkClientMaxRetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(Constants.PunkClientBackoffBase, retryAttempt)));

}