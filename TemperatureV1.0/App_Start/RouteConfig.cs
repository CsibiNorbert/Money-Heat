using System.Web.Mvc;
using System.Web.Routing;

namespace TemperatureV1._0
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx");


            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Customer", action = "Login", id = UrlParameter.Optional}
            );
        }
    }
}