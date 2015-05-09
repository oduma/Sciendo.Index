using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Models.Playlist.LastFm
{
    public class Info
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }
        
        [JsonProperty("perPage")]
        public string PerPage { get; set; }
        
        [JsonProperty("totalPages")]
        public string TotalPages { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }
    }
}
