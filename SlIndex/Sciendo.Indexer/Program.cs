using System;
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
                result = CommandLine.Parser.Default.ParseArguments<Options>(args);
                if (result.Errors.Any())
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
            Indexer.Client.IndexerAgentClient client= new IndexerAgentClient();
            if (result.Value.IndexType == IndexingType.Music)
            {
                var response = client.IndexMusicOnDemand(new IndexMusicOnDemandRequest {fromPath = result.Value.Path});
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
