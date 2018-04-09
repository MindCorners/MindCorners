using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MindCorners.RestfullService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //name: "Default",
            //routeTemplate: "api/{controller}/{action}/{id}",
            //defaults: new
            //{
            //    id = RouteParameter.Optional
            //}
            //);
        }
    }
}
