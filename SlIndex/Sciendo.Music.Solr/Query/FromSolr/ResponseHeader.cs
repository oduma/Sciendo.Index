using Newtonsoft.Json;

namespace Sciendo.Music.Solr.Query.FromSolr
{
    public class ResponseHeader
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        public int QTime { get; set; }
    }
}
