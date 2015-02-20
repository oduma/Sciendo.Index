using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sciendo.Indexer.Monitor.Client;

namespace Sciendo.Indexer.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                IIndexerAgent svc = new IndexerAgentClient();
                var response = svc.GetLastProcessedPackages();
                foreach (var progressStatus in response)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Id:" + progressStatus.Id);
                    if (progressStatus.Status == Status.Done)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(" Status: " +progressStatus.Status);
                    Console.ResetColor();
                    Console.WriteLine("Details: "+ progressStatus.Package);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("============================");
                Console.ResetColor();
                Thread.Sleep(2000);

            } while (true);
        }
    }
}
