using System.ServiceProcess;
using Sciendo.Music.Agent;

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
                new MusicAgent() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
