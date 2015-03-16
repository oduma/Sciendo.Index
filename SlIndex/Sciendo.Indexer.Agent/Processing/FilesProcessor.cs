using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using Sciendo.Music.Agent.Common;
using Sciendo.Music.Agent.Service.Solr;

namespace Sciendo.Music.Agent.Processing
{
    public abstract class FilesProcessor<TIn>:FilesProcessorBase<TIn>
    {
        protected ISolrSender Sender { private get; set; }

        protected string CatalogLetter(string musicFile, string rootFolder)
        {
            return musicFile.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { Path.DirectorySeparatorChar })[1];
        }


        public override void ProcessFilesBatch(IEnumerable<string> files, Action<Status,string> progressEvent)
        {
            LoggingManager.Debug("Starting process batch of files " +files.Count());
            var package = TransformFiles(files,TransformToDocument).ToArray();
            if (Sender != null)
            {
                var response = Sender.TrySend(package);
                if (progressEvent != null)
                    progressEvent(response.Status, JsonConvert.SerializeObject(package));
            }
            Counter += package.Length;
            LoggingManager.Debug("Processed batch of "+ package.Length +" files.");
        }

        protected abstract Document TransformToDocument(TIn transfromFrom, string file);
    }
}
