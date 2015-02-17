using System;
using System.ServiceModel;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System.IO;

namespace Sciendo.Indexer.Agent
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class IndexerAgentService:IIndexerAgent
    {
        private readonly ISolrSender _solrSender;
        private ILyricsDeserializer _lyricsDeserializer;
        private readonly MusicFilesProcessor _musicFilesProcessor;
        private string _musicRootFolder;
        private string _lyricsRootFolder;
        private string _musicSearchPattern;
        private string _lyricsSearchPattern;

        public IndexerAgentService(
            ISolrSender solrSender, 
            ILyricsDeserializer lyricsDeserializer,
            MusicFilesProcessor musicFilesProcessor,
            string musicRootFolder, 
            string lyricsRootFolder,
            string musicSearchPattern,
            string lyricsSearchPattern)
        {
            _solrSender = solrSender;
            _lyricsDeserializer = lyricsDeserializer;
            _musicFilesProcessor = musicFilesProcessor;
            _musicRootFolder = musicRootFolder;
            _lyricsRootFolder = lyricsRootFolder;
            _musicSearchPattern = musicSearchPattern;
            _lyricsSearchPattern = lyricsSearchPattern;
            _musicFilesProcessor.SolrSender = _solrSender;

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
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(forMusicPath,_lyricsDeserializer);
            lyricsFileProcessor.SolrSender = _solrSender;
            reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, _lyricsSearchPattern);
            return lyricsFileProcessor.Counter;
        }

        public int IndexMusicOnDemand(string fromPath)
        {
            _musicFilesProcessor.ResetCounter();
            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFilesProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath,_musicSearchPattern);
            return _musicFilesProcessor.Counter;

        }
    }
}
