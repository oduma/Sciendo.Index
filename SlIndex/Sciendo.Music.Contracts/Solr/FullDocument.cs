using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
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

        public FullDocument():base()
        {
            
        }

        internal FullDocument(string filePath, string rootFolder, string[] artists, string songTitle, string albumName, string lyrics):this(filePath,rootFolder,artists,songTitle,albumName)
        {
            Lyrics = new Field<string> {Set = lyrics};
        }
    }
}
