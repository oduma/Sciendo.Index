using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders
{
    public class Highlighting
    {
        [JsonProperty("lyrics")]
        public string[] Lyrics { get; set; }
        [JsonProperty("title")]
        public string[] Title { get; set; }
        [JsonProperty("album")]
        public string[] Album { get; set; }
        [JsonProperty("artist")]
        public string[] Artist { get; set; }
    }
}
