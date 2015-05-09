using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class Track:LovedItem
    {
        [JsonProperty("date")]
        public LovedDate LovedDate { get; set; }

        [JsonProperty("artist")]
        public LovedItem LovedArtist { get; set; }
        
        [JsonProperty("streamable")]
        public Streamable Streamable { get; set; }

    }
}
