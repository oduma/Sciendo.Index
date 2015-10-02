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
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Solr.Query;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR;
using Sciendo.Music.Real.Feedback;
using Sciendo.Music.Real.Analysis;

namespace Sciendo.Music.Agent
{
    public partial class MusicAgent : ServiceBase
    {
        private IMusic _musicService;
        private IAnalysis _analysisService;
        private ServiceHost[] _agentServiceHosts;
        private AgentConfigurationSection _agentConfigurationSection;
        private MonitoringInstance _monitoringInstance;
        public MonitoringInstance MonitoringInstance { get { return _monitoringInstance; } }
        public IMusic AgentService { get { return _musicService; } }
        public IAnalysis AnalysisService { get { return _analysisService; } }

        public MusicAgent()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();
            Init();
            LoggingManager.Debug("Agent constructed.");
        }

        internal void Init()
        {
            _agentConfigurationSection = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");
            LoggingManager.Debug(_agentConfigurationSection.ToString());
            RegisterIocComponents();
            _monitoringInstance = new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.CurrentMonitoringComponentKey));
        }

        internal void RegisterIocComponents()
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
                ResolveComponents(_agentConfigurationSection.CurrentProcessingComponentKey);
            }
            catch (Exception ex)
            {
                LoggingManager.LogSciendoSystemError("Agent not started.", ex);
                throw;
            }

            OpenServiceHost();
            OpenSignalRHub();
            StartMonitoringInstances();
            LoggingManager.Debug("Agent started.");
        }

        private void OpenSignalRHub()
        {
            WebApp.Start<Startup>(_agentConfigurationSection.FeedbackUrl);
        }

        internal void ResolveComponents(string currentMusicComponentKey)
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

            _musicService = new MusicService(mProc,
                m2LProc);
            _analysisService = new AnalysisService(mProc.CurrentConfiguration.Music.SourceDirectory,
                m2LProc.CurrentConfiguration.Lyrics.SourceDirectory,
                mProc.CurrentConfiguration.Music.SearchPattern,CurrentStatisticsActivity.Instance, new Utils(new SolrResultsProvider()));
        }

        private void StartMonitoringInstances()
        {
            LoggingManager.Debug("Starting Monitoring...");
            _monitoringInstance.CancellationTokenSource = new CancellationTokenSource();
            _monitoringInstance.CancellationToken = _monitoringInstance.CancellationTokenSource.Token;
            _monitoringInstance.CancellationToken.Register(_monitoringInstance.FolderMonitor.Stop);
            _monitoringInstance.FolderMonitor.ProcessFile = _musicService.Index;
            Task monitoringTask = new Task(_monitoringInstance.FolderMonitor.Start, _monitoringInstance.CancellationToken);
            monitoringTask.Start();
            LoggingManager.Debug("Monitoring started.");
        }

        private void OpenServiceHost()
        {
            try
            {
                LoggingManager.Debug("Opening service hosts...");
                if(_agentServiceHosts != null)
                    foreach(var agentServiceHost in _agentServiceHosts)
                        if (agentServiceHost != null)
                            agentServiceHost.Close();

                _agentServiceHosts = new ServiceHost[] {new ServiceHost(_musicService), new ServiceHost(_analysisService)};
                foreach(var agentServiceHost in _agentServiceHosts)
                    agentServiceHost.Open();
                LoggingManager.Debug("Service hosts opened.");
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
            
            if(_agentServiceHosts!= null)
                foreach (var agentServiceHost in _agentServiceHosts)
                {
                    if (agentServiceHost != null)
                    {
                        agentServiceHost.Close();
                    }
                }
            
            LoggingManager.Debug("Agent stopped.");
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            if(powerStatus==PowerBroadcastStatus.ResumeSuspend)
            {
                LoggingManager.Debug(string.Format("Music Service: {0}; {1}; ",_musicService.GetSourceFolder(),_analysisService!=null));
            }
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            LoggingManager.Debug("Service change its session mode to: " + changeDescription.Reason.ToString() + " session id " + changeDescription.SessionId);
        }
    }
}
