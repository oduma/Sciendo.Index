using Newtonsoft.Json;

namespace Sciendo.Music.Solr.Query.Common
{
    public class Doc
    {
        [JsonProperty("file_path_id")]
        public string FilePathId { get; set; }
        [JsonProperty("file_path")]
        public string FilePath { get; set; }
        [JsonProperty("artist")]
        public string[] Artist { get; set; }
        [JsonProperty("album")]
        public string Album { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("lyrics")]
        public string Lyrics { get; set; }
    }
}