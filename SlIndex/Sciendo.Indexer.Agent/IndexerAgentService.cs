using System;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    public class IndexerAgentService:IIndexerAgent
    {
        private readonly SolrSender _solrSender;

        public IndexerAgentService(SolrSender solrSender)
        {
            _solrSender = solrSender;
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

        public int IndexLyricsOnDemand(string fromPath, string lyricsSearchPattern)
        {
            Reader reader = new Reader(ProgressEvent);
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(fromPath,_solrSender);
            reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, lyricsSearchPattern);
            return lyricsFileProcessor.Counter;
        }

        public int IndexMusicOnDemand(string fromPath, string musicSearchPattern)
        {
            MusicFilesProcessor _musicFileProcessor = new MusicFilesProcessor(_solrSender);

            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, musicSearchPattern);
            return _musicFileProcessor.Counter;

        }
    }
}
