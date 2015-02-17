﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    public abstract class FilesProcessor
    {
        public ISolrSender SolrSender { protected get; set; }
        public int Counter { get; protected set; }

        protected FilesProcessor()
        {
            Counter = 0;
        }

        public void ResetCounter()
        {
            Counter = 0;
        }

        public virtual void ProcessFilesBatch(IEnumerable<string> files, string rootFolder,Action<Status,string> progressEvent)
        {
            var package = PrepareDocuments(files, rootFolder).ToArray();
            if (SolrSender != null)
            {
                var response = SolrSender.TrySend(package);
                if (progressEvent != null)
                    progressEvent(response.Status, JsonConvert.SerializeObject(package));
            }
            Counter += package.Length;
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder);

    }
}
