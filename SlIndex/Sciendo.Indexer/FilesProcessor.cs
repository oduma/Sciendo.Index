using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer
{
    public abstract class FilesProcessor
    {
        public int Counter { get; protected set; }

        private IndexerConfigurationSection _indexerConfiguration;
        protected FilesProcessor()
        {
            Counter = 0;
            _indexerConfiguration = ((IndexerConfigurationSection)ConfigurationManager.GetSection("indexer"));
        }

        public virtual void ProcessFilesBatch(IEnumerable<string> files, string rootFolder,Action<Status,string> progressEvent)
        {
            var package = PrepareDocuments(files, rootFolder).ToArray();
            var response = SolrSender.TrySend(_indexerConfiguration.SolrConnectionString, package);
            if (progressEvent != null)
                progressEvent(response.Status, JsonConvert.SerializeObject(package));
            Counter += package.Length;
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder);

    }
}
