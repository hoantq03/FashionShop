using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mcvhoconline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "PayPal",
            url: "PayPal/{action}",
            defaults: new { controller = "PayPal", action = "Index" }
            );

            routes.MapRoute(
            name: "SignOut",
            url: "GoogleOAuth/SignOut",
            defaults: new { controller = "GoogleOAuth", action = "SignOut" }
            );
        }
    }
}
