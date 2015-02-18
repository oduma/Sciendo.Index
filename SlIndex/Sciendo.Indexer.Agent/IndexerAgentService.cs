using System;
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

        public IndexerAgentService( 
            FilesProcessor musicFilesProcessor,
            LyricsFilesProcessor lyricsFilesProcessor)
        {
            LoggingManager.Debug("Constructing IndexerAgentService...");

            _musicFilesProcessor = musicFilesProcessor;
            _lyricsFilesProcessor = lyricsFilesProcessor;
            LoggingManager.Debug("IndexerAgentService constructed.");
        }

        private static void ProgressEvent(Status arg1, string arg2)
        {
            if (arg1 != Status.Done)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("error indexing: ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Indexed Ok: ");
            }
            Console.ResetColor();
            Console.WriteLine(arg2);
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
            _musicFilesProcessor.ResetCounter();
            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath,_musicFilesProcessor.CurrentConfiguration.SearchPattern);
            return _musicFilesProcessor.Counter;

        }
    }
}
