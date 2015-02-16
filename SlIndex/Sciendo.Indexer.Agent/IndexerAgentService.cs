using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    internal class IndexerAgentService:IIndexerAgent
    {
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
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(fromPath);
            reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, lyricsSearchPattern);
            return lyricsFileProcessor.Counter;
        }

        public int IndexMusicOnDemand(string fromPath, string musicSearchPattern)
        {
            MusicFilesProcessor _musicFileProcessor = new MusicFilesProcessor();

            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFileProcessor.ProcessFilesBatch;
            reader.ParsePath(fromPath, musicSearchPattern);
            return _musicFileProcessor.Counter;

        }
    }
}
