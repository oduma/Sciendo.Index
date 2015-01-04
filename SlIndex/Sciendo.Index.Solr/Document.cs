using System;

using System.IO;

namespace Sciendo.Index.Solr
{
    public class Document
    {
        public Document()
        { }

        public Document (string filePath, string rootFolder)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            file_path_id = filePath;
            file_path = new Field<string> { set = "file:///" + file_path_id };
            extension_f = new Field<string> { set = Path.GetExtension(file_path_id).Replace(".","") };
            letter_catalog_f = new Field<string> { set = file_path_id.Replace(rootFolder, "").Split(new char[] { '\\' })[1] };

        }

        public Document(string filePath, string rootFolder, string[] artists, string song, string albumName):this(filePath,rootFolder)
        {
            artist = new Field<string[]> { set = artists };
            title = new Field<string> { set = song };
            album = new Field<string> { set = albumName };
        }
        public Document(string filePath, string rootFolder, string[] artists, string song, string albumName, string songLyrics)
            :this(filePath,rootFolder,artists,song,albumName)
        {
            
            lyrics = new Field<string> {set = songLyrics};
        }
        public string file_path_id { get; set; }
        public Field<string> file_path { get; set; }
        public Field<string> extension_f { get; set; }
        public Field<string> letter_catalog_f { get; set; }
        public Field<string[]> artist { get; set; }
        public Field<string> title { get; set; }
        public Field<string> album { get; set; }
        public Field<string> lyrics { get; set; }
    }
}
