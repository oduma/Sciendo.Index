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
using Sciendo.Music.Real.Procesors.Common;
using Sciendo.Music.Real.Procesors.LyricsSourced;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Agent.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MusicService:IMusic
    {
        private readonly MusicFilesProcessor _musicFilesProcessor;
        private readonly LyricsFilesProcessor _lyricsFilesProcessor;
        private readonly MusicToLyricsFilesProcessor _musicToLyricsFilesProcessor;

        private FixedSizedQueue<ProgressStatus> _progressStatuses;  

        public MusicService(MusicFilesProcessor musicFilesProcessor, LyricsFilesProcessor lyricsFilesProcessor, MusicToLyricsFilesProcessor musicToLyricsFilesProcessor, 
            int packagesRetainerLimit)
        {
            LoggingManager.Debug("Constructing IndexerAgentService...");

            _musicFilesProcessor = musicFilesProcessor;
            _lyricsFilesProcessor = lyricsFilesProcessor;
            _musicToLyricsFilesProcessor = musicToLyricsFilesProcessor;

            _progressStatuses= new FixedSizedQueue<ProgressStatus>(packagesRetainerLimit);
            LoggingManager.Debug("IndexerAgentService constructed.");
        }

        private void ProgressEvent(Status arg1, string arg2)
        {
            LoggingManager.Debug("Package: " +arg2+" status: " +arg1);
            _progressStatuses.Enqueue(new ProgressStatus{Package=arg2,Status =arg1,Id=Guid.NewGuid()});
        }

        public int IndexLyricsOnDemand(string fromPath)
        {
            LoggingManager.Debug("Starting IndexLyricsOnDemand from path:" +fromPath);
            IndexLyrics(fromPath, ProcessType.None);
            LoggingManager.Debug("IndexLyricsOnDemand on path: " + fromPath + " Counter: " + _lyricsFilesProcessor.Counter);

            return _lyricsFilesProcessor.Counter;
        }

        
        public int IndexMusicOnDemand(string fromPath)
        {
            LoggingManager.Debug("Starting IndexMusicOnDemand from path: " +fromPath);
            IndexMusic(fromPath, ProcessType.None);
            LoggingManager.Debug("IndexMusicOnDemand on path: " + fromPath + " Counter: " + _musicFilesProcessor.Counter);
            return _musicFilesProcessor.Counter;

        }

        public int IndexLyrics(string fromPath, ProcessType processType)
        {
            LoggingManager.Debug("Starting IndexLyrics from path:" + fromPath);
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (!Directory.Exists(fromPath) && !File.Exists(fromPath))
                throw new ArgumentException("Invalid path " + fromPath);

            Reader reader = new Reader(ProgressEvent);
            _lyricsFilesProcessor.ResetCounter();
            reader.ProcessFiles = _lyricsFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _lyricsFilesProcessor.CurrentConfiguration.SearchPattern,processType);
            return _lyricsFilesProcessor.Counter;
        }

        public int IndexMusic(string fromPath, ProcessType processType)
        {
            LoggingManager.Debug("Starting IndexMusic from path: " + fromPath);
            _musicFilesProcessor.ResetCounter();
            Reader reader = new Reader(ProgressEvent);
            reader.ProcessFiles = _musicFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _musicFilesProcessor.CurrentConfiguration.SearchPattern,processType);
            LoggingManager.Debug("IndexMusic on path: " + fromPath + " Counter: " + _musicFilesProcessor.Counter);
            return _musicFilesProcessor.Counter;
        }

        public int AcquireLyricsOnDemandFor(string musicPath, bool retryFailed)
        {
            LoggingManager.Debug("Starting AcquireLyricsOnDemand from path:" + musicPath);
            AcquireLyrics(musicPath,retryFailed,ProcessType.None);
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

            Reader reader = new Reader(ProgressEvent);
            _musicToLyricsFilesProcessor.ResetCounter();
            _musicToLyricsFilesProcessor.RetryExisting = retryFailed;
            reader.ProcessFiles = _musicToLyricsFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _musicToLyricsFilesProcessor.CurrentMusicConfiguration.SearchPattern,processType);

        }
        public int AcquireLyricsFor(string fromPath, ProcessType processType)
        {
            LoggingManager.Debug("Starting AcquireLyricsFor from path:" + fromPath);
            AcquireLyrics(fromPath, true, processType);
            LoggingManager.Debug("AcquiredLyricsFor on path: " + fromPath + " Counter: " + _musicToLyricsFilesProcessor.Counter);
            return _musicToLyricsFilesProcessor.Counter;
        }

        public ProgressStatus[] GetLastProcessedPackages()
        {
            LoggingManager.Debug("Starting Get last packages");
            return _progressStatuses.GetAllInQueue();
        }

        public string[] ListAvailableMusicPathsForIndexing(string fromPath)
        {
            if (string.IsNullOrEmpty(fromPath))
                fromPath = _musicFilesProcessor.CurrentConfiguration.SourceDirectory;
            return Reader.GetFiles(fromPath,
                _musicFilesProcessor.CurrentConfiguration.SearchPattern, SearchOption.TopDirectoryOnly, true).ToArray();
        }

        public string[] ListAvailableLyricsPathsForIndexing(string fromPath)
        {
            if (string.IsNullOrEmpty(fromPath))
                fromPath = _lyricsFilesProcessor.CurrentConfiguration.SourceDirectory;
            return Reader.GetFiles(fromPath,
                _lyricsFilesProcessor.CurrentConfiguration.SearchPattern, SearchOption.TopDirectoryOnly, true).ToArray();
        }

        public SourceFolders GetSourceFolders()
        {
            return new SourceFolders
            {
                Lyrics = _lyricsFilesProcessor.CurrentConfiguration.SourceDirectory,
                Music = _musicFilesProcessor.CurrentConfiguration.SourceDirectory
            };
        }

        public int UnIndexMusicOnDemand(string musicFile)
        {
            if (_musicFilesProcessor.Sender.TrySend(new DeleteDocument(musicFile)).Status == Status.Error)
            {
                ProgressEvent(Status.Error,musicFile);
                return 0;
            }
            else if(_musicFilesProcessor.Sender.TrySend(new Commit()).Status==Status.Error)
            {
                ProgressEvent(Status.Error,musicFile);
                return 0;
            }
            ProgressEvent(Status.Done,musicFile);
            return 1;
        }

        public bool DeleteLyricsFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }

        public int UnIndexLyricsOnDemand(string musicFile)
        {
            if (_lyricsFilesProcessor.Sender.TrySend(new Document(musicFile,musicFile.ToLower().Replace(_musicFilesProcessor.CurrentConfiguration.SourceDirectory.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1],null)).Status!=Status.Done)
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
                LyricsFilesProcessorType = _lyricsFilesProcessor.GetType(),
                MusicFilesProcessorType = _musicFilesProcessor.GetType(),
                MusicToLyricsFilesProcessorType = _musicToLyricsFilesProcessor.GetType()
            };
        }
    }
}
