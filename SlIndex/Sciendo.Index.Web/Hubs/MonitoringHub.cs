using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Sciendo.Index.Web.IndexingClient;

namespace Sciendo.Index.Web.Hubs
{
    public class MonitoringHub : Hub
    {
        public void Send()
        {
            do
            {
                IIndexerAgent svc = new IndexerAgentClient();
                var response = svc.GetLastProcessedPackages();
                foreach (var progressStatus in response)
                {
                    Clients.All.addNewMessageToPage("Id:" + progressStatus.Id + " Status: " + progressStatus.Status+" Details: " + progressStatus.Package);
                }
                Thread.Sleep(2000);

            } while (true);
        }
    }
}