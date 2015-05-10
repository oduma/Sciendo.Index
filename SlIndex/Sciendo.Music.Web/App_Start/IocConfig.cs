using Sciendo.IOC;
using Sciendo.IOC.Configuration;
using Sciendo.Music.DataProviders;

namespace Sciendo.Music.Web
{
    public class IocConfig
    {
        public static void RegisterComponents(Container container)
        {
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IDataProvider>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IResultsProvider>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IPlayerProcess>(LifeStyle.Transient);
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IPlaylistProvider>(LifeStyle.Transient);
        }
    }
}