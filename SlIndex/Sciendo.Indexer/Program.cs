using System;
using System.IO;
using System.Linq;
using CommandLine;
using Sciendo.Indexer.Client;

namespace Sciendo.Indexer
{
    internal class Program
    {

        private static int Main(string[] args)
        {
            ParserResult<Options> result;
            try
            {
                result = Parser.Default.ParseArguments<Options>(args);
                if (result.Errors.Any() || (!Directory.Exists(result.Value.Path) &&!File.Exists(result.Value.Path)))
                {
                    PrintHelp();
                    return -1;
                }
            }
            catch
            {
                PrintHelp();
                return -1;
            }
            var client= new IndexerAgentClient();
            if (result.Value.IndexType == IndexingType.Music)
            {
                var response = client.IndexMusicOnDemand(new IndexMusicOnDemandRequest {fromPath = result.Value.Path});
                if(Directory.Exists(result.Value.Path))
                    Console.WriteLine("Indexed {0} music files from {1}.",response.IndexMusicOnDemandResult,Directory.GetFiles(result.Value.Path,"*.*",SearchOption.AllDirectories).Count());
                else
                {
                    Console.WriteLine("Indexed {0} music files from 1.", response.IndexMusicOnDemandResult);
                }
            }
            if (result.Value.IndexType == IndexingType.Lyrics)
            {
                var response = client.IndexLyricsOnDemand(new IndexLyricsOnDemandRequest { fromPath = result.Value.Path});
                if (Directory.Exists(result.Value.Path))
                    Console.WriteLine("Indexed {0} lyrics files from {1}.", response.IndexLyricsOnDemandResult, Directory.GetFiles(result.Value.Path, "*.*", SearchOption.AllDirectories).Count());
                else
                {
                    Console.WriteLine("Indexed {0} lyrics files from 1.", response.IndexLyricsOnDemandResult);
                }
            }
            Console.ReadKey();
            return 1;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Sciendo.Indexer path -t indexingType\r\nindexingType can be: Music or Lyrics.");
        }
    }
}
