using System.Xml.Serialization;

namespace Sciendo.Lyrics.Common
{
    public class ArtistSong
    {
        [XmlIgnore]
        public string artist { get; set; }
        [XmlIgnore]
        public string song { get; set; }

    }
}
