﻿using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sciendo.IOC;
using Sciendo.Music.DataProviders;

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

        public static DataProvidersConfigurationSection IndexingConfiguration
        {
            get
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("dataProviders"))
                    HttpContext.Current.Application.Add("dataProviders", ConfigurationManager.GetSection("dataProviders"));
                return HttpContext.Current.Application["dataProviders"] as DataProvidersConfigurationSection;
            }
        }
    }
}
