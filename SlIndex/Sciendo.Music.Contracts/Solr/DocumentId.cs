using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class DocumentId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
