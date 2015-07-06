using System.Collections.Generic;

namespace Sciendo.Music.Solr.Query.FromSolr
{
    public class SolrResponse
    {
        public ResponseHeader responseHeader { get; set; }

        public Response response { get; set; }

        public Dictionary<string,Highlighting> highlighting { get; set; }

        public Facets facet_counts { get; set; }

    }
}
