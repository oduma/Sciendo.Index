using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sciendo.IOC;
using Sciendo.Music.DataProviders.Configuration;

namespace Sciendo.Music.Web
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

        public static QueryConfigurationSection QueryConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("query"))
                    HttpContext.Current.Application.Add("query", ConfigurationManager.GetSection(ConfigurationSectionNames.QueryProviderConfigurationName));
                return HttpContext.Current.Application["query"] as QueryConfigurationSection;
            }
        }

        public static IndexDataProviderConfigurationSection IndexingConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("index"))
                    HttpContext.Current.Application.Add("index", ConfigurationManager.GetSection(ConfigurationSectionNames.IndexProviderConfigurationName));
                return HttpContext.Current.Application["index"] as IndexDataProviderConfigurationSection;
            }
        }
        public static PlayerConfigurationSection PlayerConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("player"))
                    HttpContext.Current.Application.Add("player", ConfigurationManager.GetSection(ConfigurationSectionNames.PlayerProcessConfigurationName));
                return HttpContext.Current.Application["player"] as PlayerConfigurationSection;
            }
        }
        public static PlaylistProviderConfigurationSection PlaylistConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("playlist"))
                    HttpContext.Current.Application.Add("playlist", ConfigurationManager.GetSection(ConfigurationSectionNames.PlaylistProviderConfigurationName));
                return HttpContext.Current.Application["playlist"] as PlaylistProviderConfigurationSection;
            }
        }
    }
}
