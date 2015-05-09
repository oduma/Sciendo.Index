using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class Streamable
    {
        [JsonProperty("#text")]
        public string Text { get; set; }

        [JsonProperty("fulltrack")]
        public string FullTrack { get; set; }

    }
}
