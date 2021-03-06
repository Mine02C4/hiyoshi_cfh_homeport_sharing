﻿using HiyoshiCfhWeb.Models;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

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
            builder.EntitySet<Admiral>("Admirals");
            builder.EntitySet<Ship>("Ships");
            builder.EntitySet<ShipType>("ShipTypes");
            builder.EntitySet<ShipInfo>("ShipInfoes");
            builder.EntitySet<SlotItem>("SlotItems");
            builder.EntitySet<SlotItemInfo>("SlotItemInfoes");
            builder.EntitySet<Quest>("Quests");
            builder.EntitySet<MaterialRecord>("MaterialRecords");
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
