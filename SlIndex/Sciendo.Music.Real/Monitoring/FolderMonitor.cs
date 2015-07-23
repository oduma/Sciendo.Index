using System;
using System.IO;
using System.Threading;
using Sciendo.Common.IO;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.IO;

namespace Sciendo.Music.Real.Monitoring
{

    public sealed class FolderMonitor : IFolderMonitor,IDisposable
    {
        private readonly string _currentRootFolder;

        public FolderMonitor(string currentRootFolder)
        {
            LoggingManager.Debug("Constructing FolderMonitor for: " +currentRootFolder);
            if (string.IsNullOrEmpty(currentRootFolder))
                throw new ArgumentNullException("currentRootFolder");
            if (!Directory.Exists(currentRootFolder))
                throw new ArgumentException("Invalid path: " +currentRootFolder);
            _currentRootFolder = currentRootFolder;
            LoggingManager.Debug("FolderMonitor constructed for: " + currentRootFolder);

        }

        public void Stop()
        {
            LoggingManager.Debug("Stoping FolderMonitor...");
            if (_fsWatcher != null)
            {
                _fsWatcher.Stop();
            }
            More = false;
            LoggingManager.Debug("FolderMonitor stopped.");
        }

        private DirectoryMonitor _fsWatcher;

        public Action<string,ProcessType> ProcessFile { private get; set; }

        public void Start()
        {
            LoggingManager.Debug("FolderMonitor Initializing FolderWatcher...");
            if (!More)
            {
                Stop();
                _fsWatcher = new DirectoryMonitor(_currentRootFolder);
                _fsWatcher.Change += fsWatcher_Changed;
                _fsWatcher.Delete += fsWatcher_Deleted;
                _fsWatcher.Rename += fsWatcher_Renamed;
                _fsWatcher.Start();
            }
            More = true;

            LoggingManager.Debug("FolderMonitor Initialized FolderWatcher...");
        }

        private void fsWatcher_Renamed(string frompath, string topath)
        {
            if (!More)
            {
                Stop();
                return;
            }
            LoggingManager.Debug("A File renamed from " + frompath + " to " + topath);
            //if it is a directory ignore it
            if (!File.Exists(topath))
                return;
            // Wait if file is still open
            FileInfo fileInfo = new FileInfo(topath);
            while (FileAccessChecker.IsFileLocked(fileInfo))
            {
                Thread.Sleep(750);
            }

            if (ProcessFile != null)
            {
                ProcessFile(frompath,ProcessType.Delete);
                ProcessFile(topath, ProcessType.Update);
            }
        }

        private void fsWatcher_Deleted(string path)
        {
            if (!More)
            {
                Stop();
                return;
            }
            LoggingManager.Debug("A file deleted: " + path);
            if (Directory.Exists(path))
                return;
            if (ProcessFile != null)
            {
                ProcessFile(path, ProcessType.Delete);
            }
        }

        private void fsWatcher_Changed(string path)
        {
            if (!More)
            {
                Stop();
                return;
            }
            LoggingManager.Debug("A file changed: " + path);
            //if it is a directory ignore it
            if (!File.Exists(path))
                return;
            //queue an insert;
            // Wait if file is still open
            FileInfo fileInfo = new FileInfo(path);
            while (FileAccessChecker.IsFileLocked(fileInfo))
            {
                Thread.Sleep(750);
            }
            //queue an update;
            if (ProcessFile != null)
            {
                ProcessFile(path,ProcessType.Update);
            }
        }

        public bool More { get; private set; }

        public void Dispose()
        {
            Stop();
            _fsWatcher.Stop();
        }
    }
}
