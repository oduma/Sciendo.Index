using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sciendo.Indexing.DataProviders;
using Sciendo.IOC;

namespace Sciendo.Index.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.RegisterComponents(SciendoConfiguration.Container);

        }
    }

    public static class SciendoConfiguration
    {

        public static Container Container
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("ioc"))
                    HttpContext.Current.Application.Add("ioc", Container.GetInstance());
                return HttpContext.Current.Application["ioc"] as Container;
            }
        }

        public static IndexingConfigurationSection IndexingConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("indexing"))
                    HttpContext.Current.Application.Add("indexing", ConfigurationManager.GetSection("indexing"));
                return HttpContext.Current.Application["indexing"] as IndexingConfigurationSection;
            }
        }
    }
}
