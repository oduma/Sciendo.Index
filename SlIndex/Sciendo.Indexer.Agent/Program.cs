using System.ServiceProcess;

namespace Sciendo.Indexer.Agent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new IndexerAgent() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
