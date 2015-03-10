using Sciendo.Indexing.DataProviders;
using Sciendo.Indexing.DataProviders.Mock;
using Sciendo.IOC;

namespace Sciendo.Index.Web
{
    public class IocConfig
    {
        public static void RegisterComponents(Container container)
        {
            container.Add(new RegisteredType().For<MockDataProvider>().BasedOn<IDataProvider>().IdentifiedBy("mock").With(LifeStyle.Transient));
            container.Add(new RegisteredType().For<DataProvider>().BasedOn<IDataProvider>().IdentifiedBy("real").With(LifeStyle.Transient));
        }
    }
}