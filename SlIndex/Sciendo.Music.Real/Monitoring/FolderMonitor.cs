using System;
using System.IO;
using System.Threading;
using Sciendo.Common.IO;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Monitoring;

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
                throw new ArgumentException("Invalid path");
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

        static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    stream = file.Open(FileMode.Open,
                         FileAccess.Read, FileShare.None);
                else
                    stream = file.Open(FileMode.Open,
                         FileAccess.ReadWrite, FileShare.None);

            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private DirectoryMonitor _fsWatcher;

        public Func<string,ProcessType,int>[] ProcessFile { private get; set; }

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
            while (IsFileLocked(fileInfo))
            {
                Thread.Sleep(500);
            }

            if (ProcessFile != null)
            {
                foreach (var processFile in ProcessFile)
                {
                    processFile(frompath,ProcessType.Delete);
                    processFile(topath, ProcessType.Update);
                }
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
                foreach (var processFile in ProcessFile)
                {
                    processFile(path, ProcessType.Delete);
                }
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
            while (IsFileLocked(fileInfo))
            {
                Thread.Sleep(500);
            }
            //queue an update;
            if (ProcessFile != null)
            {
                foreach (var processFile in ProcessFile)
                    processFile(path,ProcessType.Update);
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
