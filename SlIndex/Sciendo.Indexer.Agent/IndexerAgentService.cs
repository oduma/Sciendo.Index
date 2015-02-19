using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using System.IO;

namespace Sciendo.Indexer.Agent
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class IndexerAgentService:IIndexerAgent
    {
        private readonly FilesProcessor _musicFilesProcessor;
        private readonly LyricsFilesProcessor _lyricsFilesProcessor;

        private Dictionary<string, Status> progressStatus;  

        public IndexerAgentService( 
            FilesProcessor musicFilesProcessor,
            LyricsFilesProcessor lyricsFilesProcessor)
        {
            LoggingManager.Debug("Constructing IndexerAgentService...");

            _musicFilesProcessor = musicFilesProcessor;
            _lyricsFilesProcessor = lyricsFilesProcessor;
            LoggingManager.Debug("IndexerAgentService constructed.");
        }

        private void ProgressEvent(Status arg1, string arg2)
        {
            LoggingManager.Debug("File: " +arg2+" status: " +arg1);
            progressStatus.Add(arg2,arg1);
        }

        public int IndexLyricsOnDemand(string fromPath)
        {
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (!Directory.Exists(fromPath) || !File.Exists(fromPath))
                throw new ArgumentException("Invalid path");

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
            progressStatus=new Dictionary<string, Status>();
            Reader reader = new Reader(ProgressEvent);
            reader.ProcessFiles = _musicFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath,_musicFilesProcessor.CurrentConfiguration.SearchPattern);
            LoggingManager.Debug("IndexMusic on path: " + fromPath + " Counter: " + _musicFilesProcessor.Counter);
            return _musicFilesProcessor.Counter;

        }
    }
}
