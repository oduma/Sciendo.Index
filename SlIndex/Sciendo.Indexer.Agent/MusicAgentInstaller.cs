using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Sciendo.Music.Agent
{
    [RunInstaller(true)]
    public partial class MusicAgentInstaller : Installer
    {
        ServiceProcessInstaller process;
        private ServiceInstaller service;

        public MusicAgentInstaller()
        {
            process=new ServiceProcessInstaller {Account = ServiceAccount.LocalSystem};
            service= new ServiceInstaller
            {
                ServiceName = "Sciendo Music Agent",
                Description = "Post Processes Music Files"
            };
#if DEBUG
            service.ServiceName = "Sciendo Music Agent (Debug)";
            service.Description = "Post Processes Music Files (Debug)";
#endif
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
