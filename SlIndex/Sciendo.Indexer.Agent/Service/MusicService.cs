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

namespace Sciendo.Music.Agent.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MusicService:IMusic
    {
        private readonly IndexingFilesProcessor _indexingFilesProcessor;
        private readonly MusicToLyricsFilesProcessor _lyricsAcquireFilesProcessor;

        private readonly FixedSizedQueue<ProgressStatus> _progressStatuses;  

        public MusicService(IndexingFilesProcessor indexingFilesProcessor, MusicToLyricsFilesProcessor lyricsAcquireFilesProcessor, 
            int packagesRetainerLimit)
        {
            LoggingManager.Debug("Constructing MusicAgentService...");

            _indexingFilesProcessor = indexingFilesProcessor;
            _lyricsAcquireFilesProcessor = lyricsAcquireFilesProcessor;

            _progressStatuses= new FixedSizedQueue<ProgressStatus>(packagesRetainerLimit);
            LoggingManager.Debug("MusicAgentService constructed.");
        }

        private void ProgressEvent(Status arg1, string arg2)
        {
            var messageId = Guid.NewGuid();
            LoggingManager.Debug("MessageId: " + messageId+ "Package: " +arg2+" status: " +arg1);
            _progressStatuses.Enqueue(new ProgressStatus{Package="Look in server side logs for the Id",Status =arg1,Id=messageId});
        }

        public int IndexOnDemand(string fromPath)
        {
            LoggingManager.Debug("Starting IndexOnDemand from path:" +fromPath);
            Index(fromPath,ProcessType.Update);
            LoggingManager.Debug("IndexOnDemand on path: " + fromPath + " Counter: " + _indexingFilesProcessor.Counter);

            return _indexingFilesProcessor.Counter;
        }

        public int Index(string fromPath,ProcessType processType)
        {
            LoggingManager.Debug("Starting Index from path: " + fromPath);
            _indexingFilesProcessor.ResetCounter();
            var reader = new Reader(ProgressEvent) {ProcessFiles = _indexingFilesProcessor.ProcessFilesBatch};
            reader.ParsePath(fromPath, _indexingFilesProcessor.CurrentConfiguration.Music.SearchPattern,processType);
            LoggingManager.Debug("Index on path: " + fromPath + " Counter: " + _indexingFilesProcessor.Counter);
            return _indexingFilesProcessor.Counter;
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
                LoggingManager.Debug("Errored while AcquiringLyricsOnDemand on path: " + musicPath + " Counter: " + _lyricsAcquireFilesProcessor.Counter);
                return _lyricsAcquireFilesProcessor.Counter;
            }
            LoggingManager.Debug("AcquiredLyricsOnDemand on path: " + musicPath + " Counter: " + _lyricsAcquireFilesProcessor.Counter);
            return _lyricsAcquireFilesProcessor.Counter;
        }

        private void AcquireLyrics(string fromPath, bool retryFailed, ProcessType processType)
        {
            LoggingManager.Debug("Starting AcquireLyrics from path:" + fromPath);
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (!Directory.Exists(fromPath) && !File.Exists(fromPath))
                throw new ArgumentException("Invalid path " + fromPath);

            var reader = new Reader(ProgressEvent);
            _lyricsAcquireFilesProcessor.ResetCounter();
            _lyricsAcquireFilesProcessor.RetryExisting = retryFailed;
            reader.ProcessFiles = _lyricsAcquireFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _lyricsAcquireFilesProcessor.CurrentConfiguration.Music.SearchPattern,processType);

        }

        public ProgressStatus[] GetLastProcessedPackages()
        {
            LoggingManager.Debug("Starting Get last packages");
            try
            {
                return _progressStatuses.GetAllInQueue();

            }
            catch(Exception ex)
            {
                LoggingManager.LogSciendoSystemError(ex);
                return null;
            }
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

        //public int UnIndexOnDemand(string musicFile)
        //{
        //    if (_indexingFilesProcessor.Sender.TrySend(new DeleteDocument(musicFile)).Status == Status.Error)
        //    {
        //        ProgressEvent(Status.Error,musicFile);
        //        return 0;
        //    }
        //    if(_indexingFilesProcessor.Sender.TrySend(new Commit()).Status==Status.Error)
        //    {
        //        ProgressEvent(Status.Error,musicFile);
        //        return 0;
        //    }
        //    ProgressEvent(Status.Done,musicFile);
        //    return 1;
        //}

        public int UnIndexOnDemand(string musicFile)
        {
            if (_indexingFilesProcessor.Sender.TrySend(new Document(musicFile,musicFile.ToLower().Replace(_indexingFilesProcessor.CurrentConfiguration.Music.SourceDirectory.ToLower(), "").Split(new[] { Path.DirectorySeparatorChar })[1],null,null,null,null)).Status!=Status.Done)
            {
                ProgressEvent(Status.Error, musicFile);
                return 0;
            }
            ProgressEvent(Status.Done, musicFile);
            return 1;
        }

        public WorkingSet GetCurrentWorkingSet()
        {
            return new WorkingSet
            {
                IndexingFilesProcessorType = _indexingFilesProcessor.GetType(),
                LyricsAcquirerFilesProcessorType = _lyricsAcquireFilesProcessor.GetType()
            };
        }
    }
}
