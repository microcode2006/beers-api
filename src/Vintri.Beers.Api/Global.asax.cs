using System.Web;
using System.Web.Http;

namespace Vintri.Beers.Api;

/// <inheritdoc />
public class WebApiApplication : HttpApplication
{
    /// <summary>
    /// Application entry point to configure api version, register filters and IoC container.
    /// </summary>
    protected void Application_Start() => GlobalConfiguration.Configure(WebApiConfig.Register);
}