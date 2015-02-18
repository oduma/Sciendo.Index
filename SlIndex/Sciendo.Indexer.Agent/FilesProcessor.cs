using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
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
            Counter = 0;
        }

        protected string CatalogLetter(string musicFile, string rootFolder)
        {
            return musicFile.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1];
        }


        public virtual void ProcessFilesBatch(IEnumerable<string> files, Action<Status,string> progressEvent)
        {
            var package = PrepareDocuments(files).ToArray();
            if (Sender != null)
            {
                var response = Sender.TrySend(package);
                if (progressEvent != null)
                    progressEvent(response.Status, JsonConvert.SerializeObject(package));
            }
            Counter += package.Length;
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files);

    }
}
