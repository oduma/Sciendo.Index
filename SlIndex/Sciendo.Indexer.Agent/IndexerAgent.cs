using Sciendo.Index.Solr;
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
        private  string _hostUrl;

        public IndexerAgent()
        {
            InitializeComponent();
            IndexerConfigurationSection indexerConfigurationSection = (IndexerConfigurationSection)ConfigurationManager.GetSection("indexer");
            _agentService = new IndexerAgentService(new SolrSender(indexerConfigurationSection.SolrConnectionString), 
                new LyricsDeserializer(), 
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
