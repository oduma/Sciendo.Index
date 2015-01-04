using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Index.Solr
{
    public class FullDocument:Document
    {
        public Field<string[]> artist { get; set; }
        public Field<string> title { get; set; }
        public Field<string> album { get; set; }

        public FullDocument(string filePath, string rootFolder, string[] artists, string song, string albumName):base(filePath,rootFolder)
        {
            artist = new Field<string[]> { set = artists };
            title = new Field<string> { set = song };
            album = new Field<string> { set = albumName };
        }

        public FullDocument(string filePath, string rootFolder,string[] artists, string song, string albumName, string songLyrics)
            : this(filePath,rootFolder,artists,song,albumName)
        {
            lyrics = new Field<string> { set = songLyrics };
        }


    }
}
