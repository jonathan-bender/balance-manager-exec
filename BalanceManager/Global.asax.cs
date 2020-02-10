using System.Web.Http;

namespace BalanceManager
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure((httpConfiguration) =>
            {
                SwaggerConfig.Register(httpConfiguration);
                WebApiConfig.Register(httpConfiguration);
                UnityConfig.Register(httpConfiguration);
            });
        }
    }
}
