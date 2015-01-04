using Sciendo.Index.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    public class PackageTrace
    {
        public Document[] Package { get; set; }

        public TrySendResponse Response { get; set; }
    }
}
