using Newtonsoft.Json;

namespace Sciendo.Indexer.Agent.Service.Solr
{
    public class FullDocument:Document
    {

        [JsonProperty("artist")]
        public Field<string[]> Artist { get; set; }
        [JsonProperty("title")]
        public Field<string> Title { get; set; }
        [JsonProperty("album")]
        public Field<string> Album { get; set; }

        public FullDocument(string filePath, string rootFolder, string[] artists, string song, string albumName):base(filePath,rootFolder)
        {
            Artist = new Field<string[]> { Set = artists };
            Title = new Field<string> { Set = song };
            Album = new Field<string> { Set = albumName };
        }

        public FullDocument(string filePath, string rootFolder,string[] artists, string song, string albumName, string songLyrics)
            : this(filePath,rootFolder,artists,song,albumName)
        {
            Lyrics = new Field<string> { Set = songLyrics };
        }

        public FullDocument():base()
        {
            
        }
    }
}
