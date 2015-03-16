using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using Sciendo.Music.Agent.LyricsProvider;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Agent.Service.Monitoring;

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
            LoggingManager.Debug("Starting IndexLyrics from path:" +fromPath);
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (!Directory.Exists(fromPath) && !File.Exists(fromPath))
                throw new ArgumentException("Invalid path " +fromPath);

            Reader reader = new Reader(ProgressEvent);
            _lyricsFilesProcessor.ResetCounter();
            reader.ProcessFiles = _lyricsFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _lyricsFilesProcessor.CurrentConfiguration.SearchPattern);
            return _lyricsFilesProcessor.Counter;
        }

        
        public int IndexMusicOnDemand(string fromPath)
        {
            LoggingManager.Debug("Starting IndexMusic from path: " +fromPath);
            _musicFilesProcessor.ResetCounter();
            Reader reader = new Reader(ProgressEvent);
            reader.ProcessFiles = _musicFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath,_musicFilesProcessor.CurrentConfiguration.SearchPattern);
            LoggingManager.Debug("IndexMusic on path: " + fromPath + " Counter: " + _musicFilesProcessor.Counter);
            return _musicFilesProcessor.Counter;

        }

        public int AcquireLyricsFor(string musicPath, bool retryFailed)
        {
            LoggingManager.Debug("Starting AcquireLyrics from path:" + musicPath);
            if (string.IsNullOrEmpty(musicPath))
                throw new ArgumentNullException("musicPath");
            if (!Directory.Exists(musicPath) && !File.Exists(musicPath))
                throw new ArgumentException("Invalid path " + musicPath);

            Reader reader = new Reader(ProgressEvent);
            _musicToLyricsFilesProcessor.ResetCounter();
            _musicToLyricsFilesProcessor.RetryExisting = retryFailed;
            reader.ProcessFiles = _musicToLyricsFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(musicPath, _musicToLyricsFilesProcessor.CurrentMusicConfiguration.SearchPattern);
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
    }
}
