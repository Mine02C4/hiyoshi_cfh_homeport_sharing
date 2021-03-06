﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HiyoshiCfhWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Ship",
                url: "Ship/{id}",
                defaults: new { controller = "Ship", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Headquarters",
                url: "Headquarters/{id}/{action}/{param}",
                defaults: new { controller = "Headquarters", action = "Homeport", param = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
