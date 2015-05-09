using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class LovedDate
    {
        [JsonProperty("#text")]
        public string Text { get; set; }

        [JsonProperty("uts")]
        public string Uts { get; set; }
    }
}
