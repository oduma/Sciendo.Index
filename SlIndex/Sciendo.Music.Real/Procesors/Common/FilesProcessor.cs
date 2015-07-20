﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Real.Feedback;
using System.Threading;

namespace Sciendo.Music.Real.Procesors.Common
{
    public abstract class FilesProcessor<TIn>:FilesProcessorBase<TIn>
    {
        public ISolrSender Sender { get; set; }

        protected string CatalogLetter(string musicFile, string rootFolder)
        {
            return musicFile.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1];
        }

        
        public override void ProcessFilesBatch(IEnumerable<string> files)
        {
            LoggingManager.Debug("Starting process batch of files " +files.Count());
            DeletedDocuments=new List<DeleteDocument>();
            var package = TransformFiles(files,TransformToDocument).Where(p=>p!=null).ToArray();
            if (Sender != null)
            {
                if (package != null && package.Length > 0)
                {
                    var response = Sender.TrySend(package);
                }
                foreach (var deletedDocument in DeletedDocuments)
                {
                    var response = Sender.TrySend(deletedDocument);
                    if (response.Status == Status.Done)
                    {
                        Counter++;
                    }
                }
                if(package.Length>0 && DeletedDocuments.Count()>0)
                {
                    var commit = new Commit();
                    var commitResponse = Sender.TrySend(commit);
                    
                }
            }
            Counter += package.Length;
            LoggingManager.Debug("Processed batch of "+ package.Length +" files.");
        }

        public List<DeleteDocument> DeletedDocuments { get; set; }

        protected abstract Document TransformToDocument(TIn songLyrics, string file);
    }
}
