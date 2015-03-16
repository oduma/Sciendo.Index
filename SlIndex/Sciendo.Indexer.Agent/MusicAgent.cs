using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Processing;
using Sciendo.Indexer.Agent.Processing.Mocks;
using Sciendo.IOC;
using Sciendo.Music.Agent.LyricsProvider;
using Sciendo.Music.Agent.LyricsProvider.Mocks;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Agent.Processing.Mocks;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Agent.Service.Monitoring;
using Sciendo.Music.Agent.Service.Monitoring.Mocks;

namespace Sciendo.Music.Agent
{
    public partial class MusicAgent : ServiceBase
    {
        private MusicService _agentService;
        private ServiceHost _agentServiceHost;
        private AgentConfigurationSection _agentConfigurationSection;
        private Dictionary<MonitoringType,MonitoringInstance> _monitoringInstances=new Dictionary<MonitoringType, MonitoringInstance>();

        public MusicAgent()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();
            _agentConfigurationSection = (AgentConfigurationSection)ConfigurationManager.GetSection("indexer");
            LoggingManager.Debug(_agentConfigurationSection.ToString());
            RegisterIOCComponents();
            CreateMonitoringInstances();
            LoggingManager.Debug("Agent constructed.");
        }

        private void CreateMonitoringInstances()
        {
            LoggingManager.Debug("Creating monitoring instances...");

            _monitoringInstances.Add(MonitoringType.Music, new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.Music.CurrentMonitoringImplementation)));
            _monitoringInstances.Add(MonitoringType.Lyrics, new MonitoringInstance(
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_agentConfigurationSection.Lyrics.CurrentMonitoringImplementation)));
            LoggingManager.Debug("Monitoring instances created.");
        }

        private void RegisterIOCComponents()
        {
            LoggingManager.Debug("Starting Registration of IOC Components...");
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MockMusicFilesProcessor>()
                        .BasedOn<MusicFilesProcessor>()
                        .IdentifiedBy("mock")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MusicFilesProcessor>()
                        .BasedOn<MusicFilesProcessor>()
                        .IdentifiedBy("real")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(new RegisteredType().For<MockLyricsFilesProcessor>()
                    .BasedOn<LyricsFilesProcessor>()
                    .IdentifiedBy("mock")
                    .With(LifeStyle.Transient)
                    .WithConstructorParameters(
                        _agentConfigurationSection.Lyrics.SourceDirectory, _agentConfigurationSection.Music.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<LyricsFilesProcessor>()
                        .BasedOn<LyricsFilesProcessor>()
                        .IdentifiedBy("real")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MockFolderMonitor>()
                        .BasedOn<IFolderMonitor>()
                        .IdentifiedBy("mockmusic")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_agentConfigurationSection.Music.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<FolderMonitor>()
                        .BasedOn<IFolderMonitor>()
                        .IdentifiedBy("realmusic")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_agentConfigurationSection.Music.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MockFolderMonitor>()
                        .BasedOn<IFolderMonitor>()
                        .IdentifiedBy("mocklyrics")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_agentConfigurationSection.Lyrics.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<FolderMonitor>()
                        .BasedOn<IFolderMonitor>()
                        .IdentifiedBy("reallyrics")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_agentConfigurationSection.Lyrics.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MockDownloader>()
                        .BasedOn<WebDownloaderBase>()
                        .IdentifiedBy("mock")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<WebDownloader>()
                        .BasedOn<WebDownloaderBase>()
                        .IdentifiedBy("real")
                        .With(LifeStyle.Transient));
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

            _agentService = new MusicService(IOC.Container.GetInstance().Resolve<MusicFilesProcessor>(_agentConfigurationSection.Music.CurrentProcessingImplementation),
                IOC.Container.GetInstance()
                    .Resolve<LyricsFilesProcessor>(_agentConfigurationSection.Lyrics.CurrentProcessingImplementation), 
                    IOC.Container.GetInstance()
                    .Resolve<MusicToLyricsFilesProcessor>(_agentConfigurationSection.Lyrics.CurrentProcessingImplementation),_agentConfigurationSection.PackagesRetainerLimit);
            OpenServiceHost();
            StartMonitoringInstances();
            LoggingManager.Debug("Agent started.");
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
            _monitoringInstances[MonitoringType.Music].FolderMonitor.ProcessFile = _agentService.IndexMusicOnDemand;
            _monitoringInstances[MonitoringType.Lyrics].FolderMonitor.ProcessFile = _agentService.IndexLyricsOnDemand;
            LoggingManager.Debug("Monitoring instances prepared.");
        }

        private void OpenServiceHost()
        {
            LoggingManager.Debug("Opening service host...");
            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = new ServiceHost(_agentService);
            _agentServiceHost.Open();
            LoggingManager.Debug("Service host opened.");
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
