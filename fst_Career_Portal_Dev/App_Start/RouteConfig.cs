using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace fst_Career_Portal_Dev
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

//            routes.MapRoute(
//    name: "ProfileDocType",
//    url: "Profile/GetDropdownOptionsFromDatabase_DocType/{loggedUser}",
//    defaults: new { controller = "Profile", action = "GetDropdownOptionsFromDatabase_DocType" }
//);
        }
    }
}
