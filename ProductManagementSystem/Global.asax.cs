using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProductManagementSystem
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Register areas
            AreaRegistration.RegisterAllAreas();
            
            // Register routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
} 