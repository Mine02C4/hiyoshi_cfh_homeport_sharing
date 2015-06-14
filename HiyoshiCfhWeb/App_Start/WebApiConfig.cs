using HiyoshiCfhWeb.Models;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace HiyoshiCfhWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // ベアラ トークン認証のみを使用するように、Web API を設定します。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ShipType>("ShipTypes");
            builder.EntitySet<ShipInfo>("ShipInfoes");
            builder.EntitySet<Admiral>("Admirals");
            builder.EntitySet<Ship>("Ships");
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
