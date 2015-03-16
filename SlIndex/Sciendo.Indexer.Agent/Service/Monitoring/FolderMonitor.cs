using System;
using System.IO;
using System.Threading;
using Sciendo.Common.Logging;

namespace Sciendo.Music.Agent.Service.Monitoring
{
    public interface IFolderMonitor
    {
        void Stop();
        Func<string,int> ProcessFile { set; }
        bool More { get;}
        void Start();
    }

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
                _fsWatcher.EnableRaisingEvents = false;
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

        private FileSystemWatcher _fsWatcher;

        public Func<string,int> ProcessFile { private get; set; }

        public void Start()
        {
            LoggingManager.Debug("FolderMonitor Initializing FolderWatcher...");
            if (!More)
            {
                Stop();
                _fsWatcher = new FileSystemWatcher(_currentRootFolder);
                _fsWatcher.Created += _fsWatcher_Created;
                _fsWatcher.EnableRaisingEvents = true;
            }
            More = true;

            LoggingManager.Debug("FolderMonitor Initialized FolderWatcher...");
        }

        private void _fsWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!More)
            {
                Stop();
                return;
            }
            LoggingManager.Debug("A file created: " + e.FullPath);
            //if it is a directory ignore it
            if (!File.Exists(e.FullPath))
                return;
            //queue an insert;
            // Wait if file is still open
            FileInfo fileInfo = new FileInfo(e.FullPath);
            while (IsFileLocked(fileInfo))
            {
                Thread.Sleep(500);
            }
            if(ProcessFile!=null)
                ProcessFile(e.FullPath);
        }

        public bool More { get; private set; }

        public void Dispose()
        {
            Stop();
            _fsWatcher.Dispose();
        }
    }
}
