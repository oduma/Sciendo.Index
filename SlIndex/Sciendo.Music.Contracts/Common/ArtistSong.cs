using System.Xml.Serialization;

namespace Sciendo.Music.Contracts.Common
{
    public class ArtistSong
    {
        [XmlIgnore]
        public string artist { get; set; }
        [XmlIgnore]
        public string song { get; set; }

    }
}
