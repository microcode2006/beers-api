using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Vintri.Beers.Core;

namespace Vintri.Beers.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
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