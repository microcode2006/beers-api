using System.Web.Http;
using Microsoft.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using Vintri.Beers.Api;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Vintri.Beers.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion(ApiVersion.Default.MajorVersion.ToString(), "Vintri Beers API");
                    c.UseFullTypeNameInSchemaIds();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi();
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\Vintri.Beers.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
