using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http;

namespace BalanceManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            // Web API configuration and services
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            httpConfiguration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
