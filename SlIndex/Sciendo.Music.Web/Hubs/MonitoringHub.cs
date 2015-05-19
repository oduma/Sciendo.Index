using System.Threading;
using Microsoft.AspNet.SignalR;
using Sciendo.Music.DataProviders;
using Newtonsoft.Json;
using Sciendo.Music.DataProviders.Models.Indexing;
using System;
using Sciendo.Common.Logging;

namespace Sciendo.Music.Web.Hubs
{
    public class MonitoringHub : Hub
    {
        public void ToggleSending(bool on)
        {
            IndexingCacheData.ContinueMonitoring = on;
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
           
            return base.OnDisconnected(stopCalled);
        }
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

            } while (IndexingCacheData.ContinueMonitoring);
            // ReSharper disable once FunctionNeverReturns
        }
    }
}