using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            MusicFilesProcessor _musicFileProcessor = new MusicFilesProcessor();
            LyricsFilesProcessor lyricsFileProcessor = new LyricsFilesProcessor(args[0]);

            Reader reader = new Reader();
            if (args[1].ToLower() != "*.lrc")
            {
                reader.ProcessFiles = _musicFileProcessor.ProcessFilesBatch;
                reader.ParseDirectory(args[0], args[1]);
                Console.WriteLine("Music files indexed: {0}", _musicFileProcessor.Counter);
            }
            else
            {
                reader.ProcessFiles = lyricsFileProcessor.ProcessFilesBatch;
                reader.ParseDirectory(args[2], args[1]);
                Console.WriteLine("Lyrics files indexed: {0}", lyricsFileProcessor.Counter);
            }
            Console.ReadLine();

        }
    }
}
