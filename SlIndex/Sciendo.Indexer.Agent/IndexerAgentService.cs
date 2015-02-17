using System;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System.IO;

namespace Sciendo.Indexer.Agent
{
    public class IndexerAgentService:IIndexerAgent
    {
        private readonly SolrSender _solrSender;
        private LyricsDeserializer _lyricsDeserializer;
        private string _musicRootFolder;
        private string _lyricsRootFolder;
        private string _musicSearchPattern;
        private string _lyricsSearchPattern;

        public IndexerAgentService(SolrSender solrSender, LyricsDeserializer lyricsDeserializer,
            string musicRootFolder, 
            string lyricsRootFolder,
            string musicSearchPattern,
            string lyricsSearchPattern)
        {
            _solrSender = solrSender;
            _lyricsDeserializer = lyricsDeserializer;
            _musicRootFolder = musicRootFolder;
            _lyricsRootFolder = lyricsRootFolder;
            _musicSearchPattern = musicSearchPattern;
            _lyricsSearchPattern = lyricsSearchPattern;
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
            var forMusicPath = Path.GetDirectoryName(fromPath).Replace(_lyricsRootFolder, _musicRootFolder);
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(forMusicPath,_solrSender,_lyricsDeserializer);
            reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _lyricsSearchPattern);
            return lyricsFileProcessor.Counter;
        }

        public int IndexMusicOnDemand(string fromPath)
        {
            MusicFilesProcessor _musicFileProcessor = new MusicFilesProcessor(_solrSender);

            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath,_musicSearchPattern);
            return _musicFileProcessor.Counter;

        }
    }
}
