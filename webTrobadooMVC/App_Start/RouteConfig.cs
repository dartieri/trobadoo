using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace webTrobadooMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Concepto",
                url: "Concepto",
                defaults: new { controller = "Concept", action = "Index" }
            );
            routes.MapRoute(
                name: "Concept",
                url: "Concept",
                defaults: new { controller = "Concept", action = "Index" }
            );

            routes.MapRoute(
                name: "Servicios",
                url: "Servicios",
                defaults: new { controller = "Services", action = "Index" }
            );
            routes.MapRoute(
                name: "Services",
                url: "Services",
                defaults: new { controller = "Services", action = "Index" }
            );

            routes.MapRoute(
                name: "Contacto",
                url: "Contacto",
                defaults: new { controller = "Contact", action = "Index" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "Contact",
                defaults: new { controller = "Contact", action = "Index" }
            );

            routes.MapRoute(
                name: "Valoracion",
                url: "Valoracion",
                defaults: new { controller = "Contact", action = "Valuate" }
            );
            routes.MapRoute(
                name: "Valuate",
                url: "Valuate",
                defaults: new { controller = "Contact", action = "Valuate" }
            );

            routes.MapRoute(
                name: "Productos",
                url: "Productos",
                defaults: new { controller = "Products", action = "Index" }
            );
            routes.MapRoute(
                name: "Products",
                url: "Products",
                defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}