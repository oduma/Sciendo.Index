using Sciendo.Index.Solr;
using Sciendo.IOC;
using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer.Agent
{
    public partial class IndexerAgent : ServiceBase
    {
        private IndexerAgentService _agentService;
        private ServiceHost _agentServiceHost;

        public IndexerAgent()
        {
            InitializeComponent();
            IndexerConfigurationSection indexerConfigurationSection = (IndexerConfigurationSection)ConfigurationManager.GetSection("indexer");

            
            IOC.Container.GetInstance().Add(new RegisteredType().For<MockSender>().BasedOn<ISolrSender>().IdentifiedBy("mockSender").With(LifeStyle.Transient).WithConstructorParameters("something"));
            IOC.Container.GetInstance().Add(new RegisteredType().For<SolrSender>().BasedOn<ISolrSender>().IdentifiedBy("solrSender").With(LifeStyle.Transient).WithConstructorParameters(indexerConfigurationSection.SolrConnectionString));
            IOC.Container.GetInstance().Add(new RegisteredType().For<MockLyricsDeserializer>().BasedOn<ILyricsDeserializer>().IdentifiedBy("mockLyrics").With(LifeStyle.Transient));
            IOC.Container.GetInstance().Add(new RegisteredType().For<LyricsDeserializer>().BasedOn<ILyricsDeserializer>().IdentifiedBy("lyrics").With(LifeStyle.Transient));

            _agentService = new IndexerAgentService(IOC.Container.GetInstance().Resolve<ISolrSender>(indexerConfigurationSection.CurrentSender), 
                IOC.Container.GetInstance()
                    .Resolve<ILyricsDeserializer>(indexerConfigurationSection.Lyrics.CurrentImplementation), 
                indexerConfigurationSection.Music.SourceDirectory, 
                indexerConfigurationSection.Lyrics.SourceDirectory, 
                indexerConfigurationSection.Music.SearchPattern, 
                indexerConfigurationSection.Lyrics.SearchPattern);
        }

        protected override void OnStart(string[] args)
        {
            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = new ServiceHost(_agentService);
            _agentServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (_agentServiceHost != null)
                _agentServiceHost.Close();
            _agentServiceHost = null;
        }
    }
}
