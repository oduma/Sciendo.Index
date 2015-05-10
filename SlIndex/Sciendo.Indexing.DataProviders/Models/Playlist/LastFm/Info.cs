using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class Info
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }
        
        [JsonProperty("perPage")]
        public int PerPage { get; set; }
        
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
