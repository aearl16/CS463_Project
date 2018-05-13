using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LandingPad
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

            routes.MapRoute(name: "signin-google", url: "signin-google", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-facebook", url: "signin-facebook", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-linkedin", url: "signin-linkedin", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-twitter", url: "signin-twitter", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-instagram", url: "signin-instagram", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-orchid", url: "signin-orchid", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-deviantart", url: "signin-deviantart", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "signin-dropbox", url: "signin-dropbox", defaults: new { controller = "Account", action = "LoginCallback" });
            routes.MapRoute(name: "login-twitter", url:"signin-twitter", defaults: new { controller = "Home", action = "TwitterCallback" });
        }
    }
}
