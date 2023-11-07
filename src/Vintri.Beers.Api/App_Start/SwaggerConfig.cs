using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using Vintri.Beers.Api;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), nameof(SwaggerConfig.Register))]

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Configure Swagger
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Enable Swagger with UI and XML comments
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Vintri Beers API");
                    c.UseFullTypeNameInSchemaIds();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi();
        }

        private static string GetXmlCommentsPath() => $@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\Vintri.Beers.Api.XML";
    }
}
