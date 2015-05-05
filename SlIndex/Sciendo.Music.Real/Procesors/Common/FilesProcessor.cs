﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;

namespace Sciendo.Music.Real.Procesors.Common
{
    public abstract class FilesProcessor<TIn>:FilesProcessorBase<TIn>
    {
        public ISolrSender Sender { get; set; }

        protected string CatalogLetter(string musicFile, string rootFolder)
        {
            return musicFile.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1];
        }

        
        public override void ProcessFilesBatch(IEnumerable<string> files, Action<Status,string> progressEvent)
        {
            LoggingManager.Debug("Starting process batch of files " +files.Count());
            DeletedDocuments=new List<DeleteDocument>();
            var package = TransformFiles(files,TransformToDocument).ToArray();
            if (Sender != null)
            {
                var response = Sender.TrySend(package);
                if (progressEvent != null)
                {
                    if (response == null || package == null || package.Length == 0 ||
                        string.IsNullOrEmpty(package[0].FilePathId))
                        progressEvent(Status.Error, "bogus package or unknown error");
                    else
                        progressEvent(response.Status, JsonConvert.SerializeObject(package));
                }
                foreach (var deletedDocument in DeletedDocuments)
                {
                    response = Sender.TrySend(deletedDocument);
                    if (progressEvent != null)
                        progressEvent(response.Status, JsonConvert.SerializeObject(deletedDocument));
                    Counter++;
                }
                var commit = new Commit();
                response = Sender.TrySend(commit);
                if (progressEvent != null)
                    progressEvent(response.Status, (response.Status==Status.Done)?"Committed":"Not committed");
            }
            Counter += package.Length;
            LoggingManager.Debug("Processed batch of "+ package.Length +" files.");
        }

        public List<DeleteDocument> DeletedDocuments { get; set; }

        protected abstract Document TransformToDocument(TIn songLyrics, string file);
    }
}
