using Newtonsoft.Json;
using Sciendo.Music.Solr.Query.Common;

namespace Sciendo.Music.Solr.Query.FromSolr
{
    public class Response
    {
        [JsonProperty("numFound")]
        public int NumFound { get; set; }
        [JsonProperty("start")]
        public int Start { get; set; }
        [JsonProperty("docs")]
        public Doc[] Docs { get; set; }
    }
}
