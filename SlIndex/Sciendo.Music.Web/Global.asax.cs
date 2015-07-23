using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sciendo.Common.Logging;
using Sciendo.IOC;
using Sciendo.Music.DataProviders.Configuration;
using Sciendo.Music.Solr.Configuration;

namespace Sciendo.Music.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //LoggingManager.Debug("Application starting...");
            //LoggingManager.Debug(SciendoConfiguration.ToString());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.RegisterComponents(SciendoConfiguration.Container);
            //LoggingManager.Debug("Application started.");
        }
    }

    public static class SciendoConfiguration
    {
        public static new string ToString()
        {
            return
                string.Format(
                    "With configuration:\r\nFor querying:{0}\r\nFor indexing:{1}\r\nFor Playing:{2}\r\nFor Playlists:{3}\r\nFor Statistics:{4}\r\nFor Feedback:{5} ",
                    QueryConfiguration.ToString(), IndexingConfiguration.ToString(), PlayerConfiguration.ToString(),
                    PlaylistConfiguration.ToString(),StatisticsConfiguration.ToString(),FeedbackConfiguration.ToString());
        }
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
                    HttpContext.Current.Application.Add("query", ConfigurationManager.GetSection(QueryConfigurationName.QueryProviderConfigurationName));
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
        public static StatisticsProviderConfigurationSection StatisticsConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("statistics"))
                    HttpContext.Current.Application.Add("statistics", ConfigurationManager.GetSection(ConfigurationSectionNames.StatisticsProviderConfigurationName));
                return HttpContext.Current.Application["statistics"] as StatisticsProviderConfigurationSection;
            }
        }
        public static FeedbackProviderConfigurationSection FeedbackConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("feedback"))
                    HttpContext.Current.Application.Add("feedback", ConfigurationManager.GetSection(ConfigurationSectionNames.FeedbackProviderConfigurationName));
                return HttpContext.Current.Application["feedback"] as FeedbackProviderConfigurationSection;
            }
        }
    }
}
