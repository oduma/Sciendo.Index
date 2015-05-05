using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class SolrResponseHeader
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("QTime")]
        public int Time { get; set; }
    }
}
