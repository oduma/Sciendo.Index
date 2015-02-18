using Sciendo.Common.Logging;
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

        public IndexerAgent()
        {
            LoggingManager.Debug("Constructing the Agent...");
            InitializeComponent();

            LoggingManager.Debug("Agent constructed.");
        }

        protected override void OnStart(string[] args)
        {
            LoggingManager.Debug("Starting the Agent...");
            IndexerConfigurationSection indexerConfigurationSection = (IndexerConfigurationSection)ConfigurationManager.GetSection("indexer");


            IOC.Container.GetInstance().Add(new RegisteredType().For<MockMusicFilesProcessor>().BasedOn<FilesProcessor>().IdentifiedBy("mock").With(LifeStyle.Transient));
            IOC.Container.GetInstance().Add(new RegisteredType().For<MusicFilesProcessor>().BasedOn<FilesProcessor>().IdentifiedBy("real").With(LifeStyle.Transient));
            IOC.Container.GetInstance().Add(new RegisteredType().For<MockLyricsFilesProcessor>().BasedOn<LyricsFilesProcessor>().IdentifiedBy("mock").With(LifeStyle.Transient).WithConstructorParameters(
                indexerConfigurationSection.Lyrics.SourceDirectory,indexerConfigurationSection.Music.SourceDirectory));
            IOC.Container.GetInstance().Add(new RegisteredType().For<LyricsFilesProcessor>().BasedOn<LyricsFilesProcessor>().IdentifiedBy("real").With(LifeStyle.Transient));

            LoggingManager.Debug(indexerConfigurationSection.ToString());

            _agentService = new IndexerAgentService(IOC.Container.GetInstance().Resolve<FilesProcessor>(indexerConfigurationSection.Music.CurrentImplementation),
                IOC.Container.GetInstance()
                    .Resolve<LyricsFilesProcessor>(indexerConfigurationSection.Lyrics.CurrentImplementation));
            LoggingManager.Debug("Opening service host...");
            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = new ServiceHost(_agentService);
            _agentServiceHost.Open();
            LoggingManager.Debug("Service host opened.");
            LoggingManager.Debug("Agent started.");
        }

        protected override void OnStop()
        {
            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = null;
        }
    }
}
