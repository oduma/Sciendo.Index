using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class LovedTracks
    {
        [JsonProperty("track")]
        public Track[] Tracks { get; set; }

        [JsonProperty("@attr")]
        public Info Info { get; set; }
    }
}
