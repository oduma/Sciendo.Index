using Sciendo.Indexer.Agent;
using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    class Program
    {

        static void Main(string[] args)
        {
            IndexerConfigurationSection indexerConfigurationSection = (IndexerConfigurationSection)ConfigurationManager.GetSection("indexer");
            MusicFilesProcessor _musicFileProcessor = new MusicFilesProcessor();
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(indexerConfigurationSection.Music.SourceDirectory);

            Reader reader = new Reader(ProgressEvent);

            reader.ProcessFiles = _musicFileProcessor.ProcessFilesBatch;
            reader.ParsePath(indexerConfigurationSection.Music.SourceDirectory, indexerConfigurationSection.Music.SearchPattern);
            Console.WriteLine("Music files indexed: {0}", _musicFileProcessor.Counter);

            reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
            reader.ParsePath(indexerConfigurationSection.Lyrics.SourceDirectory, indexerConfigurationSection.Lyrics.SearchPattern);
            Console.WriteLine("Lyrics files indexed: {0}", lyricsFileProcessor.Counter);
            Console.ReadLine();

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
    }
}
