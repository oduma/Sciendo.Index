using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer
{
    public abstract class FilesProcessor
    {
        public int Counter { get; protected set; }

        protected FilesProcessor()
        {
            Counter = 0;
        }

        public virtual void ProcessFilesBatch(IEnumerable<string> files, string rootFolder)
        {
            var package = PrepareDocuments(files, rootFolder).ToArray();
            var response = SolrSender.TrySend("http://localhost:8080/solr/medialib/update/json?commitWithin=1000", package);
            if (response.Status != Status.Done)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("error indexing: {0}", JsonConvert.SerializeObject(package));
            Console.ResetColor();
            Counter += package.Length;
        }

        protected abstract IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder);

    }
}
