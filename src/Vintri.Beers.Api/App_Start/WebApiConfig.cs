using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Configure Web Api
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register IoC container, API version and exception handling filter
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            IocConfig.Register();
            RegisterApiVersioning(config);
            config.Filters.Add(new ExceptionHandlingAttribute());
        }

        private static void RegisterApiVersioning(HttpConfiguration config)
        {
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap = {[Constants.ApiVersion] = typeof(ApiVersionRouteConstraint)}
            };

            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
    }
}