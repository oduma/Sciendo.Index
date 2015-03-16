using System.Threading;

namespace Sciendo.Music.Agent.Service.Monitoring
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
