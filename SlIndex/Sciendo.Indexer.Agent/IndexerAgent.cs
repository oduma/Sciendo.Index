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
            _hostUrl=string.Format("http://{0}/{1}/",
                        System.Net.Dns.
                            GetHostName().ToLower(),"indexeragent");
        }

        protected override void OnStart(string[] args)
        {
            if (_agentServiceHost == null)
            {
                _agentServiceHost = new ServiceHost(_agentService,new Uri(_hostUrl));
                _agentServiceHost.AddServiceEndpoint(typeof(IIndexerAgent),new );

            serviceEndPoint.Behaviors.Add(new SciendoAuditBehavior());

            return serviceHost;

            OpenAllServiceHosts();

        }

        protected override void OnStop()
        {
        }
    }
}
