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
            file_path_id = filePath.ToLower();
            file_path = new Field<string> { set = "file:///" + filePath };

            extension_f = new Field<string> { set = Path.GetExtension(file_path_id).Replace(".", "") };
            letter_catalog_f = new Field<string> { set = file_path_id.ToLower().Replace(rootFolder.ToLower(), "").Split(new char[] { '\\' })[1] };

        }

        public Document(string filePath, string rootFolder, string songLyrics)
            :this(filePath,rootFolder)
        {
            
            lyrics = new Field<string> {set = songLyrics};
        }
        public string file_path_id { get; set; }
        public Field<string> file_path { get; set; }
        public Field<string> extension_f { get; set; }
        public Field<string> letter_catalog_f { get; set; }
        public Field<string> lyrics { get; set; }
    }
}
