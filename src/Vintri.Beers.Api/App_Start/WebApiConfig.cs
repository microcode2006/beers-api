using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.Http.Routing;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Configure Web Api
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register IoC container, API version, swagger, logging and exception handling filter
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            var serviceCollection = IocConfig.Register();
            var beersLogger = serviceCollection.BuildServiceProvider().GetRequiredService<IBeersLogger>();

            RegisterApiVersioning(config);
            RegisterSwagger(config);

            config.Filters.Add(new ExceptionHandlingAttribute(beersLogger));
            config.Filters.Add(new LogRequestAttribute(beersLogger));

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void RegisterApiVersioning(HttpConfiguration config)
        {
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    [Constants.ApiVersion] = typeof(ApiVersionRouteConstraint)
                }
            };

            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning();
        }

        private static void RegisterSwagger(HttpConfiguration config)
        {
            var xmlCommentsFilePath = $@"{AppContext.BaseDirectory}\bin\{Assembly.GetExecutingAssembly().GetName().Name}.XML";

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Vintri Beers API");
                c.UseFullTypeNameInSchemaIds();
                c.IncludeXmlComments(xmlCommentsFilePath);
            }).EnableSwaggerUi();
        }
    }
}