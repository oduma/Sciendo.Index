using System.Threading;

namespace Sciendo.Indexer.Agent.Monitoring
{
    public class MonitoringInstance
    {
        public IFolderMonitor FolderMonitor { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public MonitoringInstance(IFolderMonitor folderMonitor)
        {
            FolderMonitor = folderMonitor;
        }

    }
}
