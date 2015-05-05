using System;
using Sciendo.Lyrics.Common;
using CommandLine;
using System.Linq;
using System.IO;
using Sciendo.Lyrics.Provider.Service;

namespace Sciendo.Lyrics.Provider
{
    class Program
    {
        static Pipeline _pipeline;
        static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ParserResult<Options> result;
            try
            {
                result = Parser.Default.ParseArguments<Options>(args);
                if (result.Errors.Any() || (!Directory.Exists(result.Value.SourceDirectory) && !Directory.Exists(result.Value.TargetDirectory)))
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
            try
            {
                _pipeline = new Pipeline(result.Value.SourceDirectory, result.Value.TargetDirectory, new WebDownloader(),result.Value.TraceFile);
                Console.WriteLine("Trace saved at: {0}", _pipeline
                    .ContinueProcessing(true, Pipeline.ReadWriteContextProvider, ConsoleRecordProgress, new LyricsDeserializer())
                    .SaveTrace());
            }
            catch (NoExecutionContextException neex)
            {
                Console.WriteLine(neex.Message);
                return -1;
            }
            return 0;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _pipeline.SaveTrace();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Sciendo.Lyrics.Provider.exe sourcedirectory targetdirectory [tracefile]");
        }

        private static void ConsoleRecordProgress(Status status, string arg2, string arg3)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(status);
            Console.ResetColor();
            Console.Write(":{0}", arg2);
            if(!string.IsNullOrEmpty(arg3))
            {
                Console.ResetColor();
                Console.Write(". Additional info exists: ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(arg3);
            }
            Console.ResetColor();
            Console.WriteLine(".");
        }
    }
}
