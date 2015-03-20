using System.ServiceProcess;

namespace Sciendo.Music.Agent
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
