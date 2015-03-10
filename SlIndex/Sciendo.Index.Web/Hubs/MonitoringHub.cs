using System.Threading;
using Microsoft.AspNet.SignalR;
using Sciendo.Indexing.DataProviders;

namespace Sciendo.Index.Web.Hubs
{
    public class MonitoringHub : Hub
    {
        public void Send()
        {
            do
            {

                foreach (var progressStatus in SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .GetMonitoring())
                {
                    Clients.All.addNewMessageToPage(progressStatus);
                }
                Thread.Sleep(2000);

            } while (true);
// ReSharper disable once FunctionNeverReturns
        }
    }
}