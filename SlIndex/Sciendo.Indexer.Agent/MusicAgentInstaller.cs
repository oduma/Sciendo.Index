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
            process=new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service= new ServiceInstaller();
            service.ServiceName = "Sciendo Music Agent";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
