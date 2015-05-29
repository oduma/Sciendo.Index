using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.SolrContracts
{
    public class ResponseHeader
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        public int QTime { get; set; }
    }
}
