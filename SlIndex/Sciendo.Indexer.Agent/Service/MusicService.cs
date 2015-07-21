using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Sciendo.Common.Logging;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Agent.Service.Monitoring;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Contracts.MusicService;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Real.Procesors.MusicSourced;
using Sciendo.Music.Real.Feedback;
using System.Threading;

namespace Sciendo.Music.Agent.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MusicService:IMusic
    {
        private readonly IndexingFilesProcessor _indexingFilesProcessor;
        private readonly MusicToLyricsFilesProcessor _musicToLyricsFilesProcessor;

        public MusicService(IndexingFilesProcessor indexingFilesProcessor, MusicToLyricsFilesProcessor lyricsAcquireFilesProcessor)
        {
            LoggingManager.Debug("Constructing MusicAgentService...");

            _indexingFilesProcessor = indexingFilesProcessor;
            _musicToLyricsFilesProcessor = lyricsAcquireFilesProcessor;

            LoggingManager.Debug("MusicAgentService constructed.");
        }

        public int IndexOnDemand(string fromPath)
        {
            LoggingManager.Debug("Starting IndexOnDemand from path:" +fromPath);
            Index(fromPath,ProcessType.Update);
            LoggingManager.Debug("IndexOnDemand on path: " + fromPath + " Counter: " + _indexingFilesProcessor.Counter);

            return _indexingFilesProcessor.Counter;
        }

        public void Index(string fromPath,ProcessType processType)
        {
            LoggingManager.Debug("Starting Index from path: " + fromPath);
            _indexingFilesProcessor.ResetCounter();
            CurrentIndexingActivity.Instance.SetAndBroadcast(fromPath, ActivityStatus.Starting);
            var reader = new Reader(CurrentIndexingActivity.Instance) {ProcessFiles = _indexingFilesProcessor.ProcessFilesBatch};
            reader.ParsePath(fromPath, _indexingFilesProcessor.CurrentConfiguration.Music.SearchPattern,processType);
            CurrentIndexingActivity.Instance.BroadcastDetails("Total Files indexed: " + _indexingFilesProcessor.Counter);
            Thread.Sleep(500);
            CurrentIndexingActivity.Instance.SetAndBroadcast(fromPath, ActivityStatus.Stopped);
            Thread.Sleep(500);
            CurrentIndexingActivity.Instance.ClearAndBroadcast();
            LoggingManager.Debug("Index on path: " + fromPath + " Counter: " + _indexingFilesProcessor.Counter);
        }

        public int AcquireLyricsOnDemandFor(string musicPath, bool retryFailed)
        {
            LoggingManager.Debug("Starting AcquireLyricsOnDemand from path:" + musicPath);
            try
            {
                AcquireLyrics(musicPath, retryFailed, ProcessType.Update);

            }
            catch(Exception ex)
            {
                LoggingManager.LogSciendoSystemError(ex);
                LoggingManager.Debug("Errored while AcquiringLyricsOnDemand on path: " + musicPath + " Counter: " + _musicToLyricsFilesProcessor.Counter);
                return _musicToLyricsFilesProcessor.Counter;
            }
            LoggingManager.Debug("AcquiredLyricsOnDemand on path: " + musicPath + " Counter: " + _musicToLyricsFilesProcessor.Counter);
            return _musicToLyricsFilesProcessor.Counter;
        }

        private void AcquireLyrics(string fromPath, bool retryFailed, ProcessType processType)
        {
            LoggingManager.Debug("Starting AcquireLyrics from path:" + fromPath);
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (!Directory.Exists(fromPath) && !File.Exists(fromPath))
                throw new ArgumentException("Invalid path " + fromPath);
            CurrentGetLyricsActivity.Instance.SetAndBroadcast(fromPath, ActivityStatus.Starting);
            var reader = new Reader(CurrentGetLyricsActivity.Instance);
            _musicToLyricsFilesProcessor.ResetCounter();
            _musicToLyricsFilesProcessor.RetryExisting = retryFailed;
            reader.ProcessFiles = _musicToLyricsFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _musicToLyricsFilesProcessor.CurrentConfiguration.Music.SearchPattern,processType);
            CurrentGetLyricsActivity.Instance.BroadcastDetails("Total Lyrics Acquired: " + _musicToLyricsFilesProcessor.Counter);
            Thread.Sleep(500);
            CurrentGetLyricsActivity.Instance.SetAndBroadcast(fromPath, ActivityStatus.Stopped);
            Thread.Sleep(500);
            CurrentGetLyricsActivity.Instance.ClearAndBroadcast();
        }

        public string[] ListAvailablePathsForIndexing(string fromPath)
        {
            if (string.IsNullOrEmpty(fromPath))
                fromPath = _indexingFilesProcessor.CurrentConfiguration.Music.SourceDirectory;
            return Reader.GetFiles(fromPath,
                _indexingFilesProcessor.CurrentConfiguration.Music.SearchPattern, SearchOption.TopDirectoryOnly, true).ToArray();
        }

        public string GetSourceFolder()
        {
            return _indexingFilesProcessor.CurrentConfiguration.Music.SourceDirectory;
        }

        public int UnIndexOnDemand(string musicFile)
        {
            if (_indexingFilesProcessor.Sender.TrySend(new Document(musicFile,musicFile.ToLower().Replace(_indexingFilesProcessor.CurrentConfiguration.Music.SourceDirectory.ToLower(), "").Split(new[] { Path.DirectorySeparatorChar })[1],null,null,null,null)).Status!=Status.Done)
            {
                LoggingManager.Debug(string.Format("{0} - {1}",Status.Error, musicFile));
                return 0;
            }
            return 1;
        }

        public WorkingSet GetCurrentWorkingSet()
        {
            return new WorkingSet
            {
                IndexingFilesProcessorType = _indexingFilesProcessor.GetType(),
                LyricsAcquirerFilesProcessorType = _musicToLyricsFilesProcessor.GetType()
            };
        }
    }
}
