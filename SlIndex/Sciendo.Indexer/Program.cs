using System;
using System.Linq;

namespace Sciendo.Indexer
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
                if (result.Errors.Any())
                    PrintHelp();
            }
            catch
            {
                PrintHelp();
            }
            Console.ReadKey();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Sciendo.Indexer path -t indexingType\r\nindexingType can be: Music or Lyrics.");
        }
    }
}
