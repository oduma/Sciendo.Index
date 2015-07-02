using Sciendo.Common.Logging;
using Sciendo.IOC;
using Sciendo.IOC.Configuration;
using Sciendo.Music.DataProviders;

namespace Sciendo.Music.Web
{
    public class IocConfig
    {
        public static void RegisterComponents(Container container)
        {
            LoggingManager.Debug("Registering Ioc Components starting...");
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IDataProvider>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IResultsProvider>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IPlayerProcess>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IPlaylistProvider>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IStatisticsProvider>(LifeStyle.Transient);
            LoggingManager.Debug("Registering Ioc Components finished.");
        }
    }
}