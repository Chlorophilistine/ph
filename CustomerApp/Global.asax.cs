using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace CustomerApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MefContainer.RegisterMefDependencyResolver();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
