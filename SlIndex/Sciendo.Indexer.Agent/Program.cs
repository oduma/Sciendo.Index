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
            ServicesToRun[0].AutoLog = true;
            ServicesToRun[0].CanHandlePowerEvent = true;
            ServicesToRun[0].CanHandleSessionChangeEvent = true;
            ServicesToRun[0].CanPauseAndContinue = false;
            ServiceBase.Run(ServicesToRun);
        }
    }
}
