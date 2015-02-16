using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    public abstract class FilesProcessor
    {
        private SolrSender _solrSender;
        public int Counter { get; protected set; }

        protected FilesProcessor(SolrSender solrSender)
        {
            Counter = 0;
            _solrSender = solrSender;
        }

        public virtual void ProcessFilesBatch(IEnumerable<string> files, string rootFolder,Action<Status,string> progressEvent)
        {
            var package = PrepareDocuments(files, rootFolder).ToArray();
            var response = _solrSender.TrySend(package);
            if (progressEvent != null)
                progressEvent(response.Status, JsonConvert.SerializeObject(package));
            Counter += package.Length;
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder);

    }
}
