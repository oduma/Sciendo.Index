using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.IOC;

namespace Sciendo.Index.Web.App_Start
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