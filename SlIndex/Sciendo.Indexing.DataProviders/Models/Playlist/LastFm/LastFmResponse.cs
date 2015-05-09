using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class LastFmResponse
    {
        [JsonProperty("lovedtracks")]        
        public LovedTracks LovedTracks { get; set; }
    }
}
