using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Sciendo.Common.Logging;
using Sciendo.IOC;
using Sciendo.IOC.Configuration;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Agent.Service.Monitoring;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Contracts.MusicService;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Agent
{
    public partial class MusicAgent : ServiceBase
    {
        private IMusic _agentService;
        private ServiceHost _agentServiceHost;
        private readonly AgentConfigurationSection _agentConfigurationSection;
        private MonitoringInstance _monitoringInstance;
        public MonitoringInstance MonitoringInstance { get { return _monitoringInstance; } }
        public IMusic AgentService { get { return _agentService; } }
        public MusicAgent()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();
            _agentConfigurationSection = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");
            LoggingManager.Debug(_agentConfigurationSection.ToString());
            RegisterIocComponents();
            _monitoringInstance = new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.CurrentMonitoringComponentKey));
            LoggingManager.Debug("Agent constructed.");
        }

        private void RegisterIocComponents()
        {
            LoggingManager.Debug("Starting Registration of IOC Components...");
            var configuredContainer = IOC.Container.GetInstance().UsingConfiguration();

            configuredContainer.AddAllFromFilteredAssemblies<IndexingFilesProcessor>(LifeStyle.Transient);
            configuredContainer.AddFirstFromFilteredAssemblies<IFolderMonitor>(LifeStyle.Transient, "",
                _agentConfigurationSection.Music.SourceDirectory);
            configuredContainer.AddAllFromFilteredAssemblies<MusicToLyricsFilesProcessor>(LifeStyle.Transient);
            LoggingManager.Debug("Registered IOC Components.");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggingManager.Debug("Unhandled exception in code. See the exceptions logs.");
            LoggingManager.LogSciendoSystemError(e.ExceptionObject as Exception);
        }

        protected override void OnStart(string[] args)
        {
            LoggingManager.Debug("Starting the Agent...");

            try
            {
                ResolveComponents(_agentConfigurationSection.CurrentProcessingComponentKey,
                    _agentConfigurationSection.PackagesRetainerLimit);
            }
            catch (Exception ex)
            {
                LoggingManager.LogSciendoSystemError("Agent not started.",ex);
                throw;
            }

            OpenServiceHost();
            StartMonitoringInstances();
            LoggingManager.Debug("Agent started.");
        }

        internal void ResolveComponents(string currentMusicComponentKey,int packageRetainerlimit)
        {
            LoggingManager.Debug("Resolving current Indexing Procesor...");
            var mProc =
                IOC.Container.GetInstance()
                    .Resolve<IndexingFilesProcessor>(currentMusicComponentKey);
            LoggingManager.Debug("Current Indexing Procesor resolved.");
            LoggingManager.Debug("Resolving current Music to Lyrics Procesor...");
            var m2LProc =
                IOC.Container.GetInstance()
                    .Resolve<MusicToLyricsFilesProcessor>(currentMusicComponentKey);
            LoggingManager.Debug("Current Music To Lyrics Procesor resolved.");

            _agentService = new MusicService(mProc,
                m2LProc,
                packageRetainerlimit);
        }

        private void StartMonitoringInstances()
        {
            LoggingManager.Debug("Starting Monitoring...");
            _monitoringInstance.CancellationTokenSource = new CancellationTokenSource();
            _monitoringInstance.CancellationToken = _monitoringInstance.CancellationTokenSource.Token;
            _monitoringInstance.CancellationToken.Register(_monitoringInstance.FolderMonitor.Stop);
            _monitoringInstance.FolderMonitor.ProcessFile = new Func<string, ProcessType, int>[] { _agentService.Index };
            Task monitoringTask = new Task(_monitoringInstance.FolderMonitor.Start, _monitoringInstance.CancellationToken);
            monitoringTask.Start();
            LoggingManager.Debug("Monitoring started.");
        }

        private void OpenServiceHost()
        {
            try
            {
                LoggingManager.Debug("Opening service host...");
                if (_agentServiceHost != null)
                    _agentServiceHost.Close();
                _agentServiceHost = new ServiceHost(_agentService);
                _agentServiceHost.Open();
                LoggingManager.Debug("Service host opened.");
            }
            catch (Exception ex)
            {
                LoggingManager.LogSciendoSystemError("Hosts not opened.",ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            LoggingManager.Debug("Stoping Agent...");

            _monitoringInstance.CancellationTokenSource.Cancel();

            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = null;
            LoggingManager.Debug("Agent stopped.");
        }
    }
}
