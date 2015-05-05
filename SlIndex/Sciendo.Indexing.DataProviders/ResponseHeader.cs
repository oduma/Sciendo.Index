using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders
{
    public class ResponseHeader
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        public int QTime { get; set; }
    }
}
