using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent.Processing
{
    public abstract class FilesProcessor
    {
        protected ISolrSender Sender { private get; set; }

        public IndexerConfigurationSource CurrentConfiguration { get; protected set; }

        public int Counter { get; protected set; }

        protected FilesProcessor()
        {
            Counter = 0;
        }

        public void ResetCounter()
        {
            LoggingManager.Debug("Reseting Counter...");
            Counter = 0;
            LoggingManager.Debug("Counter reseted.");
        }

        protected string CatalogLetter(string musicFile, string rootFolder)
        {
            return musicFile.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1];
        }


        public virtual void ProcessFilesBatch(IEnumerable<string> files, Action<Status,string> progressEvent)
        {
            LoggingManager.Debug("Starting process batch of files " +files.Count());
            var package = PrepareDocuments(files).ToArray();
            if (Sender != null)
            {
                var response = Sender.TrySend(package);
                if (progressEvent != null)
                    progressEvent(response.Status, JsonConvert.SerializeObject(package));
            }
            Counter += package.Length;
            LoggingManager.Debug("Processed batch of "+ package.Length +" files.");
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files);

    }
}
