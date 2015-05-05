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
using Sciendo.Music.Real.Procesors.LyricsSourced;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Agent
{
    public partial class MusicAgent : ServiceBase
    {
        private IMusic _agentService;
        private ServiceHost _agentServiceHost;
        private readonly AgentConfigurationSection _agentConfigurationSection;
        private readonly Dictionary<MonitoringType,MonitoringInstance> _monitoringInstances=new Dictionary<MonitoringType, MonitoringInstance>();
        public Dictionary<MonitoringType, MonitoringInstance> MonitoringInstances { get { return _monitoringInstances; } }
        public IMusic AgentService { get { return _agentService; } }
        public MusicAgent()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();
            _agentConfigurationSection = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");
            LoggingManager.Debug(_agentConfigurationSection.ToString());
            RegisterIocComponents();
            CreateMonitoringInstances();
            LoggingManager.Debug("Agent constructed.");
        }

        private void CreateMonitoringInstances()
        {
            LoggingManager.Debug("Creating monitoring instances...");

            var allFolderMonitors = IOC.Container.GetInstance().ResolveAll<IFolderMonitor>();
            foreach (var type in allFolderMonitors.Select(f => f.GetType().FullName))
            {
                LoggingManager.Debug(type);
            }
            _monitoringInstances.Add(MonitoringType.Music, new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.Music.CurrentMonitoringComponentKey)));
            _monitoringInstances.Add(MonitoringType.Lyrics, new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.Lyrics.CurrentMonitoringComponentKey)));
            LoggingManager.Debug("Monitoring instances created.");
        }

        
        private void RegisterIocComponents()
        {
            LoggingManager.Debug("Starting Registration of IOC Components...");
            var configuredContainer = IOC.Container.GetInstance().UsingConfiguration();

            configuredContainer.AddAllFromFilteredAssemblies<MusicFilesProcessor>(LifeStyle.Transient);
            configuredContainer.AddAllFromFilteredAssemblies<LyricsFilesProcessor>(LifeStyle.Transient);
            configuredContainer.AddFirstFromFilteredAssemblies<IFolderMonitor>(LifeStyle.Transient, "music",
                _agentConfigurationSection.Music.SourceDirectory);
            configuredContainer.AddFirstFromFilteredAssemblies<IFolderMonitor>(LifeStyle.Transient, "lyrics",
                _agentConfigurationSection.Lyrics.SourceDirectory);
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
                ResolveComponents(_agentConfigurationSection.Music.CurrentProcessingComponentKey, _agentConfigurationSection.Lyrics.CurrentProcessingComponentKey, _agentConfigurationSection.PackagesRetainerLimit);
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

        internal void ResolveComponents(string currentMusicComponentKey,string currentLyricsComponentKey,int packageRetainerlimit)
        {
            LoggingManager.Debug("Resolving current Music Procesor...");
            var mProc =
                IOC.Container.GetInstance()
                    .Resolve<MusicFilesProcessor>(currentMusicComponentKey);
            LoggingManager.Debug("Current Music Procesor resolved.");
            LoggingManager.Debug("Resolving current Lyrics Procesor...");
            var lProc =
                IOC.Container.GetInstance()
                    .Resolve<LyricsFilesProcessor>(currentLyricsComponentKey);
            LoggingManager.Debug("Current Lyrics Procesor resolved.");
            LoggingManager.Debug("Resolving current Music to Lyrics Procesor...");
            var m2LProc =
                IOC.Container.GetInstance()
                    .Resolve<MusicToLyricsFilesProcessor>(currentLyricsComponentKey);
            LoggingManager.Debug("Current Music To Lyrics Procesor resolved.");

            _agentService = new MusicService(mProc,
                lProc,
                m2LProc,
                packageRetainerlimit);
        }

        private void StartMonitoringInstances()
        {
            LoggingManager.Debug("Starting Monitoring...");
            PrepareMonitoringInstances();
            StartingMonitoringInstances();
            LoggingManager.Debug("Monitoring started.");
        }

        private void StartingMonitoringInstances()
        {
            LoggingManager.Debug("Starting Monitoring instances...");
            foreach (var monitoringInstance in _monitoringInstances.Values)
            {
                Task monitoringTask = new Task(monitoringInstance.FolderMonitor.Start, monitoringInstance.CancellationToken);
                monitoringTask.Start();

            }
            LoggingManager.Debug("Monitoring instances started.");
        }

        private void PrepareMonitoringInstances()
        {
            LoggingManager.Debug("Preparing Monitoring instances...");
            foreach (var monitroingInstance in _monitoringInstances.Values)
            {
                monitroingInstance.CancellationTokenSource=new CancellationTokenSource();
                monitroingInstance.CancellationToken = monitroingInstance.CancellationTokenSource.Token;
                monitroingInstance.CancellationToken.Register(monitroingInstance.FolderMonitor.Stop);
            }
            _monitoringInstances[MonitoringType.Music].FolderMonitor.ProcessFile = new Func<string, ProcessType,int>[]{_agentService.IndexMusic,_agentService.AcquireLyricsFor};
            _monitoringInstances[MonitoringType.Lyrics].FolderMonitor.ProcessFile = new Func<string, ProcessType,int>[]{_agentService.IndexLyrics};
            LoggingManager.Debug("Monitoring instances prepared.");
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

            foreach(var monitoringInstance in _monitoringInstances.Values)
                monitoringInstance.CancellationTokenSource.Cancel();

            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = null;
            LoggingManager.Debug("Agent stopped.");
        }
    }

    public enum MonitoringType
    {
        None=0,
        Music=1,
        Lyrics=2
    }
}
