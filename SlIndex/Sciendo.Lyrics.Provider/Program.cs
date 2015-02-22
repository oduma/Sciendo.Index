using System;
using Sciendo.Lyrics.Common;

namespace Sciendo.Lyrics.Provider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Trace saved at: {0}", new Pipeline(args[0], args[1], args[2]).ContinueProcessing(true,Pipeline.ReadWriteContextProvider,ConsoleRecordProgress).SaveTrace());
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
