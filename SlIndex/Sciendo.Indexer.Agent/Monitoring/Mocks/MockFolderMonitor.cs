using System;
using System.IO;
using System.Linq;
using System.Threading;
using Sciendo.Common.Logging;

namespace Sciendo.Indexer.Agent.Monitoring.Mocks
{
    public class MockFolderMonitor:IFolderMonitor
    {
        private string _currentRootFolder;

        public MockFolderMonitor(string currentRootFolder)
        {
            LoggingManager.Debug("Constructing MockFolderMonitor for: " + currentRootFolder);
            _currentRootFolder = currentRootFolder;
            LoggingManager.Debug("MockFolderMonitor constructed for: " + currentRootFolder);
        }

        public void Stop()
        {
            LoggingManager.Debug("Stoping MockFolderMonitor...");
            More = false;
            LoggingManager.Debug("MockFolderMonitor stopped.");
        }

        public Func<string,int> ProcessFile { get; set; }
        public bool More { get; set; }
        public void Start()
        {
            LoggingManager.Debug("MockFolderMonitor Starting Music Monitoring...");
            var file = Directory.GetFiles(_currentRootFolder, "*.*", SearchOption.AllDirectories).FirstOrDefault();
            More = true;
            while (More)
            {
                Thread.Sleep(5000);
                ProcessFile(file);
            }
            LoggingManager.Debug("MockFolderMonitor Music Monitoring stoped.");
        }
    }
}
