using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MediaManager.Mvc
{
    /// <summary>
    /// Contains the method which defines how URLs are mapped to controller classes, their
    /// action methods and their parameters, also known as routing.
    /// </summary>
    /// <remarks>
    /// This class implements the default routing for MVC.
    /// </remarks>
    public class RouteConfig
    {
        /// <summary>
        /// Registers all the route mappings.
        /// </summary>
        /// <param name="routes">Supplied by the runtime.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
