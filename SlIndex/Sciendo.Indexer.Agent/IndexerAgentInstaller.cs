using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Sciendo.Indexer.Agent
{
    [RunInstaller(true)]
    public partial class IndexerAgentInstaller : Installer
    {
        ServiceProcessInstaller process;
        private ServiceInstaller service;

        public IndexerAgentInstaller()
        {
            process=new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service= new ServiceInstaller();
            service.ServiceName = "Indexer Agent";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
