using Newtonsoft.Json;

namespace Sciendo.Indexer.Agent.Service.Solr
{
    public class SolrResponseHeader
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("QTime")]
        public int Time { get; set; }
    }
}
