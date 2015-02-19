using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Processing;
using Sciendo.Indexer.Agent.Processing.Mocks;
using Sciendo.Indexer.Agent.Service;
using Sciendo.IOC;
using System.Configuration;
using System.ServiceModel;
using System.ServiceProcess;

namespace Sciendo.Indexer.Agent
{
    public partial class IndexerAgent : ServiceBase
    {
        private IndexerAgentService _agentService;
        private ServiceHost _agentServiceHost;
        private IFolderMonitor _musicMonitor;
        private CancellationTokenSource _musicCancellationTokenSource;
        private CancellationToken _musicCancellationToken;
        private IndexerConfigurationSection _indexerConfigurationSection;

        public IndexerAgent()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();
            _indexerConfigurationSection = (IndexerConfigurationSection)ConfigurationManager.GetSection("indexer");
            LoggingManager.Debug(_indexerConfigurationSection.ToString());
            RegisterIOCComponents();
            _musicMonitor =
                IOC.Container.GetInstance()
                    .Resolve<IFolderMonitor>(_indexerConfigurationSection.Music.CurrentMonitoringImplementation);
            LoggingManager.Debug("Agent constructed.");
        }

        private void RegisterIOCComponents()
        {
            LoggingManager.Debug("Starting Registration of IOC Components...");
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MockMusicFilesProcessor>()
                        .BasedOn<FilesProcessor>()
                        .IdentifiedBy("mock")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<MusicFilesProcessor>()
                        .BasedOn<FilesProcessor>()
                        .IdentifiedBy("real")
                        .With(LifeStyle.Transient));
            IOC.Container.GetInstance()
                .Add(new RegisteredType().For<MockLyricsFilesProcessor>()
                    .BasedOn<LyricsFilesProcessor>()
                    .IdentifiedBy("mock")
                    .With(LifeStyle.Transient)
                    .WithConstructorParameters(
                        _indexerConfigurationSection.Lyrics.SourceDirectory, _indexerConfigurationSection.Music.SourceDirectory));
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
                        .IdentifiedBy("mock")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_indexerConfigurationSection.Music.SourceDirectory));
            IOC.Container.GetInstance()
                .Add(
                    new RegisteredType().For<FolderMonitor>()
                        .BasedOn<IFolderMonitor>()
                        .IdentifiedBy("real")
                        .With(LifeStyle.Transient)
                        .WithConstructorParameters(_indexerConfigurationSection.Music.SourceDirectory));
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

            _agentService = new IndexerAgentService(IOC.Container.GetInstance().Resolve<FilesProcessor>(_indexerConfigurationSection.Music.CurrentProcessingImplementation),
                IOC.Container.GetInstance()
                    .Resolve<LyricsFilesProcessor>(_indexerConfigurationSection.Lyrics.CurrentProcessingImplementation));
            OpenServiceHost();
            StartMusicMonitoring();
            LoggingManager.Debug("Agent started.");
        }

        private void StartMusicMonitoring()
        {
            LoggingManager.Debug("Starting Music Monitoring...");
            _musicMonitor.ProcessFile = _agentService.IndexMusicOnDemand;
            _musicCancellationTokenSource = new CancellationTokenSource();
            _musicCancellationToken = _musicCancellationTokenSource.Token;
            _musicCancellationToken.Register(_musicMonitor.Stop);
            Task musicMonitoringTask = new Task(_musicMonitor.Start, _musicCancellationToken);
            musicMonitoringTask.Start();
            LoggingManager.Debug("Music Monitoring started.");
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

            _musicCancellationTokenSource.Cancel();

            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = null;
            LoggingManager.Debug("Agent stopped.");
        }
    }
}
