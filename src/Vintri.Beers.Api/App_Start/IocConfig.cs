using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Vintri.Beers.Api.Controllers;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Extensions;
using Vintri.Beers.Core.Services;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Validators;
using Vintri.Beers.Infrastructure;
using Vintri.Beers.Infrastructure.Repositories;

namespace Vintri.Beers.Api
{
    public static class IocConfig
    {
        /// Here's how to configure Microsoft DI in ASP.NET 4.6.1 and up:
        /// https://stackoverflow.com/questions/68861721/integrating-microsoft-extensions-dependencyinjection-in-asp-net-4-6-1-project
        public static void Register()
        {
            var serviceCollection = new ServiceCollection();

            RegisterTypes(serviceCollection);
            Configure(serviceCollection);
        }

        private static void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions<PunkClientSettings>()
                .Configure(opts => opts.Endpoint = ConfigurationManager.AppSettings["PunkApiEndpoint"])
                .Validate(opts => !string.IsNullOrWhiteSpace(opts.Endpoint));

            serviceCollection.AddHttpClient<IPunkClient, PunkClient>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(Constants.PunkClientLifetimeInMinutes))
                .AddPolicyHandler(GetRetryPolicy());

            serviceCollection.AddValidators();

            serviceCollection.AddScoped<IUserRatingRepository, UserRatingRepository>();
            serviceCollection.AddScoped<IBeersService, BeersService>();

            serviceCollection.AddScoped<BeersController>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(Constants.PunkClientMaxRetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(Constants.PunkClientBackoffBase, retryAttempt)));

        private static void Configure(IServiceCollection serviceCollection)
        {
            var provider = serviceCollection.BuildServiceProvider(
                new ServiceProviderOptions {
                    ValidateOnBuild = true,
                    ValidateScopes = true
                });

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new MsDiHttpControllerActivator(provider));
        }

    }

    internal class MsDiHttpControllerActivator : IHttpControllerActivator
    {
        private readonly ServiceProvider _provider;

        public MsDiHttpControllerActivator(ServiceProvider provider)
        {
            _provider = provider;
        }

        public IHttpController Create(
            HttpRequestMessage request, HttpControllerDescriptor descriptor, Type type)
        {
            var scope = _provider.CreateScope();
            request.RegisterForDispose(scope);
            return (IHttpController)scope.ServiceProvider.GetRequiredService(type);
        }
    }
}