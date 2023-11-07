using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using Vintri.Beers.Api.Controllers;
using Vintri.Beers.Core.Extensions;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Infrastructure.Extensions;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Configure Microsoft DI in ASP.NET 4.6.1 and up:
    /// https://stackoverflow.com/questions/68861721/integrating-microsoft-extensions-dependencyinjection-in-asp-net-4-6-1-project
    /// </summary>
    public static class IocConfig
    {
        /// <summary>
        /// Configure Microsoft DI and register types
        /// </summary>
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

            serviceCollection.AddValidators();
            serviceCollection.AddPunkClient();
            serviceCollection.AddUserRatingRepository();
            serviceCollection.AddBeersService();
            serviceCollection.AddScoped<BeersController>();
        }

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