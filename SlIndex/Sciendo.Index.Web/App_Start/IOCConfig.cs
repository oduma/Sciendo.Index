using Sciendo.IOC;
using Sciendo.IOC.Configuration;
using Sciendo.Music.DataProviders;

namespace Sciendo.Index.Web
{
    public class IocConfig
    {
        public static void RegisterComponents(Container container)
        {
            container.UsingConfiguration().AddAllFromFilteredAssemblies<IDataProvider>(LifeStyle.Transient);
        }
    }
}