using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Vintri.Beers.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            IocConfig.Register();
        }
    }
}